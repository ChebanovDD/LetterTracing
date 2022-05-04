using System;
using Common.Enums;
using UnityEngine;

namespace Common.Models
{
    [Serializable]
    public class StrokeData
    {
        [SerializeField] private SegmentType _type;
        [SerializeField] private bool _breakPoint;
        [SerializeField] private Vector2[] _points;

        public SegmentType Type => _type;
        public Vector2[] Points => _points;
        public bool IsBreakPoint => _breakPoint;
    }
}