using System.Collections.Generic;
using System.Windows.Media;
using System.Xml.Serialization;

namespace COMPASS.Core.Tags.Serialization
{
  [XmlRoot("Tag")]
  [XmlType("Tag")]
  public class SerializableTag
  {
    [XmlElement(ElementName = "ID")]
    public int? Id { get; set; }

    [XmlElement(ElementName = "BackgroundColor")]
    public Color? BackgroundColor { get; set; }

    [XmlElement(ElementName = "Content")]
    public string? Content { get; set; }

    [XmlElement(ElementName = "IsGroup")]
    public bool? IsGroup { get; set; }

    [XmlElement(ElementName = "ParentID")]
    public int? ParentId { get; set; }

    //[XmlElement(ElementName = "Children")]
    public List<SerializableTag?>? Children { get; set; }
  }
}