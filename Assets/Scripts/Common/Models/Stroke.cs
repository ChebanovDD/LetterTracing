using System.Collections.Generic;

namespace Common.Models
{
    public class Stroke
    {
        private readonly List<Segment> _segments = new List<Segment>();

        public IReadOnlyList<Segment> Segments => _segments;

        public void AddSegments(IEnumerable<Segment> segments)
        {
            _segments.AddRange(segments);
        }
    }
}