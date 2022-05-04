using System.Collections.Generic;
using Common.Enums;
using Common.Interfaces;
using Common.Models;
using Common.ScriptableObjects;
using Common.ViewModels;
using UnityEngine;

namespace LineRendererImplementation
{
    public class LineLetterRenderer : MonoBehaviour, ILetterRenderer
    {
        [SerializeField] private AppContext _appContext;
        
        [Space]
        [SerializeField] private Transform _canvas;
        [SerializeField] private GameObject _numberPrefab;
        [SerializeField] private GameObject _lineRendererOutlinePrefab;

        private ILetterBuilder _letterBuilder;

        private List<LineRenderer> _lineRenderers;
        private List<NumberIndicator> _numberIndicators;

        private void Awake()
        {
            _lineRenderers = new List<LineRenderer>();
            _numberIndicators = new List<NumberIndicator>();
            _letterBuilder = _appContext.Resolve<ILetterBuilder>();
        }

        public void DrawLetter(LetterData letterData)
        {
            DrawLetterOutline(letterData);
            DrawNumberIndicators(letterData);
        }

        public void Clear()
        {
            foreach (var lineRenderer in _lineRenderers)
            {
                Destroy(lineRenderer.gameObject);
            }

            foreach (var numberIndicator in _numberIndicators)
            {
                Destroy(numberIndicator.gameObject);
            }

            _lineRenderers.Clear();
            _numberIndicators.Clear();
        }

        private void DrawLetterOutline(LetterData letterData)
        {
            foreach (var stroke in letterData.Strokes)
            {
                var strokeSegments = _letterBuilder.BuildStrokeSegments(stroke);

                var count = strokeSegments.Length;
                var points = new Vector3[count + 1];

                for (var i = 0; i < count; i++)
                {
                    points[i] = strokeSegments[i].StartPoint;
                }

                points[count] = strokeSegments[count - 1].EndPoint;

                _lineRenderers.Add(CreateLine(_lineRendererOutlinePrefab, points));
            }
        }

        private LineRenderer CreateLine(GameObject lineRendererOutlinePrefab, Vector3[] points)
        {
            var lineRenderer = Instantiate(lineRendererOutlinePrefab, transform).GetComponent<LineRenderer>();
            lineRenderer.positionCount = points.Length;
            lineRenderer.SetPositions(points);

            return lineRenderer;
        }

        private void DrawNumberIndicators(LetterData letterData)
        {
            var n = 0;
            var hashSet = new HashSet<Vector2>();

            foreach (var stroke in letterData.Strokes)
            {
                foreach (var point in GetStrokePoints(stroke))
                {
                    if (hashSet.Add(point))
                    {
                        _numberIndicators.Add(CreateNumberIndicator(point, ++n));
                    }
                }
            }

            hashSet.Clear();
        }

        private IEnumerable<Vector2> GetStrokePoints(StrokeData stroke)
        {
            return stroke.Type == SegmentType.Curve ? new[] { stroke.Points[0], stroke.Points[3] } : stroke.Points;
        }

        private NumberIndicator CreateNumberIndicator(Vector2 point, int number)
        {
            var numberIndicator = Instantiate(_numberPrefab, _canvas).GetComponent<NumberIndicator>();
            numberIndicator.SetNumber(number);
            numberIndicator.transform.position = point;

            return numberIndicator;
        }
    }
}