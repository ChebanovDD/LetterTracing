using Common.Models;
using UnityEngine;

namespace Common.Interfaces
{
    public interface ISegmentGenerator
    {
        Segment[] GenerateSegments(Vector2[] points);
    }
}