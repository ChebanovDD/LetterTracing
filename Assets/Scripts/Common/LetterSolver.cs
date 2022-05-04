using System;
using System.Collections.Generic;
using Common.Interfaces;
using Common.Models;
using UnityEngine;

namespace Common
{
    public class LetterSolver : ILetterSolver
    {
        private bool _inProcess;
        private int _currentStrokeIndex;
        private int _currentSegmentIndex;

        private Stroke _currentStroke;
        private Segment _currentSegment;
        private IReadOnlyList<Stroke> _letterStrokes;

        private float _previousDistance;
        private readonly float _drawingPrecision;

        public bool InProcess => _inProcess;
        public bool IsLetterSolved => _currentStrokeIndex == _letterStrokes.Count;

        public event EventHandler<Stroke> StrokeCompleted;
        public event EventHandler<Stroke> StrokeFailed;
        public event EventHandler<Segment> SegmentChanged;

        public LetterSolver(float drawingPrecision)
        {
            _drawingPrecision = drawingPrecision;
        }

        public void SetLetter(Letter letter)
        {
            if (_inProcess)
            {
                throw new InvalidOperationException("Can not set letter while processing.");
            }

            _letterStrokes = letter.Strokes;
            _currentStrokeIndex = 0;
            _currentSegmentIndex = 0;

            ActivateStroke(0);
        }

        public bool IsStartPoint(Vector2 mousePosition)
        {
            var distance = Vector2.Distance(_currentStroke.Segments[0].StartPoint, mousePosition);

            return distance < _drawingPrecision;
        }

        public void Start(Vector2 mousePosition)
        {
            _inProcess = true;
            ActivateClosestSegment(mousePosition);
        }

        public void Stop()
        {
            if (_inProcess)
            {
                BreakProcess();
            }
        }

        public void Calculate(Vector2 mousePosition)
        {
            var projectedPoint =
                ProjectPointOnLineSegment(_currentSegment.StartPoint, _currentSegment.EndPoint, mousePosition);

            var distanceLeftToFinish = Vector2.Distance(projectedPoint, _currentSegment.EndPoint);

            if (IsMousePositionAway(mousePosition, projectedPoint) || IsWrongDirection(distanceLeftToFinish))
            {
                BreakProcess();
                return;
            }

            if (IsBackwardMove(distanceLeftToFinish))
            {
                return;
            }

            if (IsProjectedPointNextToSegmentEnd(projectedPoint, _currentSegment.EndPoint))
            {
                ActivateNextSegment();
            }
            else
            {
                _currentSegment.CurrentPoint = projectedPoint;
                _previousDistance = distanceLeftToFinish;
            }
        }

        private bool IsMousePositionAway(Vector2 mousePosition, Vector2 projectedPoint)
        {
            var mouseDistanceToLine = (projectedPoint - mousePosition).magnitude;
            return mouseDistanceToLine > _drawingPrecision;
        }

        private bool IsWrongDirection(float distanceLeftToFinish)
        {
            return distanceLeftToFinish - _previousDistance > _drawingPrecision;
        }

        private bool IsBackwardMove(float distanceLeftToFinish)
        {
            return distanceLeftToFinish > _previousDistance;
        }

        private bool IsProjectedPointNextToSegmentEnd(Vector2 projectedPoint, Vector2 segmentEndPoint)
        {
            return Vector2.Distance(projectedPoint, segmentEndPoint) < 0.1f; //_drawingPrecision;
        }

        private void BreakProcess()
        {
            _inProcess = false;
            ResetStates(_currentStroke);

            StrokeFailed?.Invoke(this, _currentStroke);
            ActivateSegment(_currentStroke, 0);
        }

        private void ResetStates(Stroke stroke)
        {
            foreach (var segment in stroke.Segments)
            {
                segment.Reset();
            }
        }

        private void CompleteStroke()
        {
            _inProcess = false;
            _currentStrokeIndex++;

            StrokeCompleted?.Invoke(this, _currentStroke);

            if (IsLetterSolved == false)
            {
                ActivateStroke(_currentStrokeIndex);
            }
        }

        private void ActivateStroke(int index)
        {
            _currentStroke = _letterStrokes[index];
            ActivateSegment(_currentStroke, 0);
        }

        private void ActivateNextSegment()
        {
            var nextSegmentIndex = _currentSegmentIndex + 1;
            if (nextSegmentIndex >= _currentStroke.Segments.Count)
            {
                CompleteStroke();
            }
            else
            {
                ActivateSegment(_currentStroke, nextSegmentIndex);
            }
        }

        private void ActivateSegment(Stroke stroke, int segmentIndex)
        {
            _currentSegmentIndex = segmentIndex;
            _currentSegment = stroke.Segments[segmentIndex];
            _previousDistance = Vector2.Distance(_currentSegment.StartPoint, _currentSegment.EndPoint);

            SegmentChanged?.Invoke(this, _currentSegment);
        }

        private void ActivateClosestSegment(Vector2 mousePosition)
        {
            var minDistanceToSegment = float.MaxValue;
            var startPoint = _currentStroke.Segments[0].StartPoint;

            foreach (var segment in _currentStroke.Segments)
            {
                var distanceToSegment = Vector2.Distance(segment.StartPoint, mousePosition);
                if (distanceToSegment > minDistanceToSegment)
                {
                    break;
                }

                minDistanceToSegment = distanceToSegment;

                var distanceToStartPoint = Vector2.Distance(startPoint, segment.EndPoint);
                if (distanceToStartPoint < _drawingPrecision)
                {
                    ActivateNextSegment();
                }
            }
        }

        private Vector2 ProjectPointOnLineSegment(Vector2 startPoint, Vector2 endPoint, Vector2 point)
        {
            var lineVector = (endPoint - startPoint).normalized;
            var projectedPoint = ProjectPointOnLine(startPoint, lineVector, point);

            return projectedPoint;
        }

        private Vector2 ProjectPointOnLine(Vector2 linePoint, Vector2 lineVector, Vector2 point)
        {
            var linePointToPoint = point - linePoint;
            var dot = Vector2.Dot(linePointToPoint, lineVector);

            return linePoint + lineVector * dot;
        }
    }
}