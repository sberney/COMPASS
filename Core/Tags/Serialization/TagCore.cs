using System.Collections.Generic;
using System.Windows.Media;

namespace COMPASS.Core.Tags.Serialization
{
    public struct TagCore : ITagCore<TagCore>
    {
        public Color BackgroundColor { get; set; }
        public string Content { get; set; }
        public bool IsGroup { get; set; }
        public int ParentId { get; set; }
        public int Id { get; set; }
        public IReadOnlyList<TagCore> Children { get; init; }
    }
}