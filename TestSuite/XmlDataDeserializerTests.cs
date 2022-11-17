using COMPASS.Core.Tags;
using COMPASS.Core.Tags.Serialization;
using System.IO.Abstractions.TestingHelpers;

namespace COMPASS.TestSuite
{
  public class XmlDataDeserializerTests
  {
    [Fact]
    public void TestTagReaderThrowsOnBadData()
    {
      var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
      {
        { @"c:\data.xml", new MockFileData("asdfasdf") },
      });

      var deserializer = new TagReader(fileSystem);
      Assert.Throws<InvalidOperationException>(() =>
      {
        deserializer.ReadTags(@"c:\data.xml");
      });
    }

    [Fact]
    public void TestTagReaderDeserializesBasicData()
    {
      var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
      {
        { @"c:\data.xml", new MockFileData(
          @"<?xml version=""1.0"" encoding=""utf-8""?>
            <ArrayOfTag xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
              <Tag>
                <Children>
                  <Tag>
                    <Children />
                    <Content>Okay</Content>
                    <ParentID>2</ParentID>
                    <BackgroundColor>
                      <A>255</A>
                      <R>255</R>
                      <G>1</G>
                      <B>65</B>
                      <ScA>1</ScA>
                      <ScR>1</ScR>
                      <ScG>0.000303527</ScG>
                      <ScB>0.05286065</ScB>
                    </BackgroundColor>
                    <IsGroup>false</IsGroup>
                    <ID>1</ID>
                  </Tag>
                  <Tag>
                    <Children />
                    <Content>SBern</Content>
                    <ParentID>2</ParentID>
                    <BackgroundColor>
                      <A>255</A>
                      <R>24</R>
                      <G>141</G>
                      <B>202</B>
                      <ScA>1</ScA>
                      <ScR>0.009134059</ScR>
                      <ScG>0.26635563</ScG>
                      <ScB>0.59061885</ScB>
                    </BackgroundColor>
                    <IsGroup>false</IsGroup>
                    <ID>0</ID>
                  </Tag>
                </Children>
                <Content>GTag</Content>
                <ParentID>-1</ParentID>
                <BackgroundColor>
                  <A>255</A>
                  <R>0</R>
                  <G>0</G>
                  <B>0</B>
                  <ScA>1</ScA>
                  <ScR>0</ScR>
                  <ScG>0</ScG>
                  <ScB>0</ScB>
                </BackgroundColor>
                <IsGroup>true</IsGroup>
                <ID>2</ID>
              </Tag>
            </ArrayOfTag>
          "
          ) },
      });

      var deserializer = new TagReader(fileSystem);
      var tags = deserializer.ReadTags(@"c:\data.xml");

      Assert.Equal(1, tags?.Root.Count);
      Assert.Equal(new List<int> { 2, 1, 0 }, tags?.All.Select(item => item.Id) ?? new List<int>());
      Assert.Equal(3, tags?.All.Count);
    }
  }
}