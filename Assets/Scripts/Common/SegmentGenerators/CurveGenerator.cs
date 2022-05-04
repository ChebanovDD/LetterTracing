using System;
using Common.Enums;
using Common.Interfaces;
using Common.Models;
using UnityEngine;

namespace Common.SegmentGenerators
{
    public class CurveGenerator : ISegmentGenerator
    {
        private readonly int _segmentCount;

        public CurveGenerator(int segmentCount)
        {
            _segmentCount = segmentCount;
        }

        public Segment[] GenerateSegments(Vector2[] points)
        {
            if (points.Length <= 1 || points.Length != 4)
            {
                throw new InvalidOperationException();
            }

            var segments = new Segment[_segmentCount];

            for (var i = 0; i < _segmentCount; i++)
            {
                var p1 = GetCurvePoint(points, (float) i / _segmentCount);
                var p2 = GetCurvePoint(points, (float) (i + 1) / _segmentCount);

                segments[i] = new Segment(p1, p2, SegmentType.Curve);
            }

            return segments;
        }

        private Vector2 GetCurvePoint(Vector2[] points, float t)
        {
            t = Mathf.Clamp01(t);
            var time = 1 - t;

            return time * time * time * points[0]
                   + 3 * time * time * t * points[1]
                   + 3 * time * t * t * points[2]
                   + t * t * t * points[3];
        }
    }
}