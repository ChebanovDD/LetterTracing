using System.Collections.Generic;

namespace Common.Models
{
    public class Letter
    {
        private readonly List<Stroke> _strokes;

        public IReadOnlyList<Stroke> Strokes => _strokes;

        public Letter(List<Stroke> strokes)
        {
            _strokes = strokes;
        }
    }
}