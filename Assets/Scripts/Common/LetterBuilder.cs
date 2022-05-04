using System.Collections.Generic;
using Common.Enums;
using Common.Interfaces;
using Common.Models;
using Common.ScriptableObjects;
using Common.SegmentGenerators;

namespace Common
{
    public class LetterBuilder : ILetterBuilder
    {
        private const int CurveSegments = 32;

        private readonly Dictionary<SegmentType, ISegmentGenerator> _segmentGenerators;

        public LetterBuilder()
        {
            _segmentGenerators = new Dictionary<SegmentType, ISegmentGenerator>
            {
                { SegmentType.Line, new LineGenerator() },
                { SegmentType.Curve, new CurveGenerator(CurveSegments) }
            };
        }

        public Letter BuildLetter(LetterData letterData)
        {
            return new Letter(BuildLetterStrokes(letterData));
        }

        public Segment[] BuildStrokeSegments(StrokeData strokeData)
        {
            return _segmentGenerators[strokeData.Type].GenerateSegments(strokeData.Points);
        }

        private List<Stroke> BuildLetterStrokes(LetterData letterData)
        {
            var stroke = new Stroke();
            var strokes = new List<Stroke> { stroke };

            foreach (var strokeData in letterData.Strokes)
            {
                if (strokeData.IsBreakPoint)
                {
                    stroke = new Stroke();
                    strokes.Add(stroke);
                }

                stroke.AddSegments(BuildStrokeSegments(strokeData));
            }

            return strokes;
        }
    }
}