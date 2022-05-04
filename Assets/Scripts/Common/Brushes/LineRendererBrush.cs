using System.Collections.Generic;
using Common.Enums;
using Common.Interfaces;
using Common.Models;
using UnityEngine;

namespace Common.Brushes
{
    public class LineRendererBrush : MonoBehaviour, IBrush
    {
        [SerializeField] private AppContext _appContext;

        [Space] [SerializeField] private GameObject _lineRendererFillPrefab;

        private ILetterEvents _letterEvents;
        private List<LineRenderer> _lineRenderers;

        private Segment _activeSegment;
        private LineRenderer _activeSegmentLine;
        private List<LineRenderer> _strokeLines;

        private void Awake()
        {
            _strokeLines = new List<LineRenderer>();
            _lineRenderers = new List<LineRenderer>();
            _letterEvents = _appContext.Resolve<ILetterEvents>();
        }

        private void OnEnable()
        {
            _letterEvents.StrokeCompleted += OnStrokeCompleted;
            _letterEvents.StrokeFailed += OnStrokeFailed;
            _letterEvents.SegmentChanged += OnSegmentChanged;
        }

        private void OnDisable()
        {
            _letterEvents.StrokeCompleted -= OnStrokeCompleted;
            _letterEvents.StrokeFailed -= OnStrokeFailed;
            _letterEvents.SegmentChanged -= OnSegmentChanged;
        }

        public void Draw(Vector2 mousePosition)
        {
            if (_activeSegmentLine != null && _activeSegment != null)
            {
                _activeSegmentLine.SetPosition(_activeSegmentLine.positionCount - 1, _activeSegment.CurrentPoint);
            }
        }

        public void Clear()
        {
            foreach (var lineRenderer in _lineRenderers)
            {
                Destroy(lineRenderer.gameObject);
            }

            _lineRenderers.Clear();
        }

        private void OnStrokeCompleted(object sender, Stroke stroke)
        {
            _activeSegmentLine.SetPosition(_activeSegmentLine.positionCount - 1, _activeSegment.EndPoint);
            _lineRenderers.AddRange(_strokeLines);

            ResetStates();
        }

        private void OnStrokeFailed(object sender, Stroke stroke)
        {
            foreach (var temp in _strokeLines)
            {
                Destroy(temp.gameObject);
            }

            ResetStates();
        }

        private void OnSegmentChanged(object sender, Segment segment)
        {
            if (_activeSegmentLine == null)
            {
                _activeSegmentLine = CreateSegmentLine();
            }
            else if (_activeSegment != null)
            {
                CompleteSegment(_activeSegmentLine, _activeSegment);

                if (_activeSegment.Type != segment.Type || segment.Type == SegmentType.Line)
                {
                    _activeSegmentLine = CreateSegmentLine();
                }
            }

            _activeSegment = segment;
            _activeSegmentLine.positionCount++;
            _activeSegmentLine.SetPosition(_activeSegmentLine.positionCount - 1, segment.StartPoint);
            _activeSegmentLine.positionCount++;
            _activeSegmentLine.SetPosition(_activeSegmentLine.positionCount - 1, segment.CurrentPoint);
        }

        private LineRenderer CreateSegmentLine()
        {
            var segmentLine = Instantiate(_lineRendererFillPrefab, transform).GetComponent<LineRenderer>();
            _strokeLines.Add(segmentLine);

            return segmentLine;
        }

        private void CompleteSegment(LineRenderer segmentLine, Segment segment)
        {
            segmentLine.SetPosition(segmentLine.positionCount - 1, segment.EndPoint);
        }

        private void ResetStates()
        {
            _strokeLines.Clear();
            _activeSegment = null;
            _activeSegmentLine = null;
        }
    }
}