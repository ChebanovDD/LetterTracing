using Common;
using Common.Enums;
using Common.Models;
using Common.ScriptableObjects;
using UnityEditor;
using UnityEngine;

public class LetterGizmo : MonoBehaviour
{
    [SerializeField] private CanvasInputSystem _inputSystem;
    [SerializeField] private float _precision = 0.5f;
    [SerializeField] private int _numberOfSegments = 32;

    [Space]
    [SerializeField] private LetterData _letterData;

    private bool _isDrawing;
    private Vector2 _mousePosition;

    private void OnEnable()
    {
        _inputSystem.MouseDown += OnMouseDown;
        _inputSystem.MouseMove += OnMouseMove;
        _inputSystem.MouseUp += OnMouseUp;
    }

    private void OnDisable()
    {
        _inputSystem.MouseDown -= OnMouseDown;
        _inputSystem.MouseMove -= OnMouseMove;
        _inputSystem.MouseUp -= OnMouseUp;
    }

    private void OnMouseDown(object sender, Vector2 position)
    {
        _isDrawing = true;
        _mousePosition = position;
    }

    private void OnMouseMove(object sender, Vector2 position)
    {
        _mousePosition = position;
    }

    private void OnMouseUp(object sender, Vector2 position)
    {
        _isDrawing = false;
    }

    private void OnDrawGizmos()
    {
        if (_letterData == null)
        {
            return;
        }

        if (_isDrawing)
        {
            Handles.color = Color.green;
            Handles.DrawWireDisc(_mousePosition, Vector3.forward, _precision);
            Handles.color = Color.white;
        }

        Handles.color = Color.yellow;
        Handles.DrawWireDisc(_letterData.Strokes[0].Points[0], Vector3.forward, _precision);
        Handles.color = Color.white;

        foreach (var strokeData in _letterData.Strokes)
        {
            var points = strokeData.Type == SegmentType.Line
                ? GenerateLineSegments(strokeData.Points)
                : GenerateCurveSegments(strokeData.Points);

            foreach (var segment in points)
            {
                Handles.DrawLine(segment.StartPoint, segment.EndPoint);
            }
        }
    }

    private Segment[] GenerateLineSegments(Vector2[] points)
    {
        var count = points.Length - 1;
        var segments = new Segment[count];

        for (var i = 0; i < count; i++)
        {
            segments[i] = new Segment(points[i], points[i + 1], SegmentType.Line);
        }

        return segments;
    }

    private Segment[] GenerateCurveSegments(Vector2[] points)
    {
        var segments = new Segment[_numberOfSegments];

        for (var i = 0; i < _numberOfSegments; i++)
        {
            var p1 = GetCurvePoint(points, (float) i / _numberOfSegments);
            var p2 = GetCurvePoint(points, (float) (i + 1) / _numberOfSegments);

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