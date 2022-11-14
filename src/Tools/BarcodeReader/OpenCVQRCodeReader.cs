using OpenCvSharp;
using OpenCvSharp.Extensions;
using ZXing.Common;

namespace COMPASS.Tools.BarcodeReader
{
  // based on https://github.com/FrancescoBonizzi/WebcamControl-WPF-With-OpenCV
  public class OpenCVQRCodeReader
  {
    private void RotateImage(Mat source, Mat destination, double angle, double scale)
    {
      Point2f imageCenter = new(source.Cols / 2f, source.Rows / 2f);
      Mat rotationMat = Cv2.GetRotationMatrix2D(imageCenter, angle, scale);
      Cv2.WarpAffine(source, destination, rotationMat, source.Size());
    }

    public string DetectBarcode(Mat mat, double rotation = 0)
    {
      // Multiple passes here to test against different thresholds
      int[] thresholds = new int[]
      {
                80, 120, 160, 200, 220
      };

      Mat originalFrame = mat.Clone();

      string barcodeText = null;
      foreach (int t in thresholds)
      {
        barcodeText = DetectBarcodeInternal(mat, t, rotation);
        if (!string.IsNullOrWhiteSpace(barcodeText))
        {
          return barcodeText;
        }
        // If I don't to this, I see a lot of squares on the frame, one for each threshold pass
        mat = originalFrame;
      }

      return barcodeText;
    }

    private string DetectBarcodeInternal(Mat mat, double threshold, double rotation = 0)
    {
      Mat image = mat;

      if (rotation != 0)
      {
        RotateImage(image, image, rotation, 1);
      }

      Mat gray = new();
      int channels = image.Channels();
      if (channels > 1)
      {
        Cv2.CvtColor(image, gray, ColorConversionCodes.BGRA2GRAY);
      }
      else
      {
        image.CopyTo(gray);
      }

      // compute the Scharr gradient magnitude representation of the images
      // in both the x and y direction
      Mat gradX = new();
      Cv2.Sobel(gray, gradX, MatType.CV_32F, xorder: 1, yorder: 0, ksize: -1);

      Mat gradY = new();
      Cv2.Sobel(gray, gradY, MatType.CV_32F, xorder: 0, yorder: 1, ksize: -1);

      // subtract the y-gradient from the x-gradient
      Mat gradient = new();
      Cv2.Subtract(gradX, gradY, gradient);
      Cv2.ConvertScaleAbs(gradient, gradient);

      // blur and threshold the image
      Mat blurred = new();
      Cv2.Blur(gradient, blurred, new Size(9, 9));

      Mat threshImage = new();
      _ = Cv2.Threshold(blurred, threshImage, threshold, 255, ThresholdTypes.Binary);

      // construct a closing kernel and apply it to the thresholded image
      Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(21, 7));
      Mat closed = new();
      Cv2.MorphologyEx(threshImage, closed, MorphTypes.Close, kernel);

      // perform a series of erosions and dilations
      Cv2.Erode(closed, closed, null, iterations: 4);
      Cv2.Dilate(closed, closed, null, iterations: 4);

      //find the contours in the thresholded image, then sort the contours
      //by their area, keeping only the largest one
      Cv2.FindContours(
          closed,
          out Point[][] contours,
          out HierarchyIndex[] hierarchyIndexes,
          mode: RetrievalModes.CComp,
          method: ContourApproximationModes.ApproxSimple);

      if (contours.Length == 0)
      {
        // throw new NotSupportedException("Couldn't find any object in the image.");
        return null;
      }

      int contourIndex = 0;
      int previousArea = 0;
      Rect biggestContourRect = Cv2.BoundingRect(contours[0]);
      while (contourIndex >= 0)
      {
        Point[] contour = contours[contourIndex];

        Rect boundingRect = Cv2.BoundingRect(contour); //Find bounding rect for each contour
        int boundingRectArea = boundingRect.Width * boundingRect.Height;
        if (boundingRectArea > previousArea)
        {
          biggestContourRect = boundingRect;
          previousArea = boundingRectArea;
        }

        contourIndex = hierarchyIndexes[contourIndex].Next;
      }

      Mat barcode = new(image, biggestContourRect); //Crop the image
      Cv2.CvtColor(barcode, barcode, ColorConversionCodes.BGRA2GRAY);

      Mat barcodeClone = barcode.Clone();
      string barcodeText = GetBarcodeText(barcodeClone);

      if (string.IsNullOrWhiteSpace(barcodeText))
      {
        int th = 100;
        _ = Cv2.Threshold(barcode, barcode, th, 255, ThresholdTypes.Tozero);
        _ = Cv2.Threshold(barcode, barcode, th, 255, ThresholdTypes.Binary);
        barcodeText = GetBarcodeText(barcode);
      }

      Cv2.Rectangle(image,
          new Point(biggestContourRect.X, biggestContourRect.Y),
          new Point(biggestContourRect.X + biggestContourRect.Width, biggestContourRect.Y + biggestContourRect.Height),
          new Scalar(0, 255, 0),
          2);

      return barcodeText;
    }

    private string GetBarcodeText(Mat barcode)
    {
      // `ZXing.Net` needs a white space around the barcode
      Mat barcodeWithWhiteSpace = new(new Size(barcode.Width + 30, barcode.Height + 30), MatType.CV_8U, Scalar.White);
      Rect drawingRect = new(new Point(15, 15), new Size(barcode.Width, barcode.Height));
      Mat roi = barcodeWithWhiteSpace[drawingRect];
      barcode.CopyTo(roi);

      return DecodeBarcodeText(barcodeWithWhiteSpace.ToBitmap());
    }

    private string DecodeBarcodeText(System.Drawing.Bitmap barcodeBitmap)
    {
      ZXing.Windows.Compatibility.BarcodeReader reader = new()
      {
        AutoRotate = true,
        Options = new DecodingOptions
        {
          TryHarder = true,
          TryInverted = true
        }
      };

      ZXing.Result result = reader.Decode(barcodeBitmap);
      return result == null ? string.Empty : result.Text;
    }

  }
}
