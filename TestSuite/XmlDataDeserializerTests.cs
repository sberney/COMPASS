using COMPASS.Core.Tags;
using System.IO.Abstractions.TestingHelpers;

namespace COMPASS.TestSuite
{
  public class XmlDataDeserializerTests
  {
    [Fact]
    public void TestTagReaderDeserializesBasicData()
    {
      var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
      {
        { @"c:\data.xml", new MockFileData("asdfasdf") },
      });

      var deserializer = new TagReader(fileSystem);
      var tags = deserializer.ReadTags(@"c:\data.xml");
    }
  }
}