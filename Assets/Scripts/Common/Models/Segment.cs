using Common.Enums;
using UnityEngine;

namespace Common.Models
{
    public class Segment
    {
        public Vector2 StartPoint { get; }
        public Vector2 EndPoint { get; }
        public Vector2 CurrentPoint { get; set; }
        public SegmentType Type { get; }

        public Segment(Vector2 startPoint, Vector2 endPoint, SegmentType type)
        {
            Type = type;
            StartPoint = startPoint;
            EndPoint = endPoint;
            CurrentPoint = startPoint;
        }

        public void Reset()
        {
            CurrentPoint = StartPoint;
        }
    }
}