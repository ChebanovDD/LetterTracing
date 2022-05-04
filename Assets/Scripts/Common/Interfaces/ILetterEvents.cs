using System;
using Common.Models;

namespace Common.Interfaces
{
    public interface ILetterEvents
    {
        public event EventHandler<Stroke> StrokeCompleted;
        public event EventHandler<Stroke> StrokeFailed;
        public event EventHandler<Segment> SegmentChanged;
    }
}