using UnityEngine;

namespace Common.Interfaces
{
    public interface IBrush
    {
        void Draw(Vector2 mousePosition);
        void Clear();
    }
}