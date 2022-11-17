using System.Windows.Media;

namespace COMPASS.Core.Tags.Serialization
{
    public interface ITagCore<out TTag> : IHasId, IHasChildren<TTag> where TTag : IHasChildren<TTag>
    {
        Color BackgroundColor { get; set; }
        string Content { get; set; }
        bool IsGroup { get; set; }
        int ParentId { get; set; }
    }

    // public class SerializableTag
}