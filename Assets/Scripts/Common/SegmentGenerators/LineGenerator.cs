using System;
using Common.Enums;
using Common.Interfaces;
using Common.Models;
using UnityEngine;

namespace Common.SegmentGenerators
{
    public class LineGenerator : ISegmentGenerator
    {
        public Segment[] GenerateSegments(Vector2[] points)
        {
            if (points.Length <= 1)
            {
                throw new InvalidOperationException();
            }

            var count = points.Length - 1;
            var segments = new Segment[count];

            for (var i = 0; i < count; i++)
            {
                segments[i] = new Segment(points[i], points[i + 1], SegmentType.Line);
            }

            return segments;
        }
    }
}