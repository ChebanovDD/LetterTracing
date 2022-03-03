using System;
using UnityEngine;

namespace Common.Interfaces
{
    public interface IInputSystem
    {
        event EventHandler<Vector2> MouseDown;
        event EventHandler<Vector2> MouseDrag;
        event EventHandler<Vector2> MouseUp;
    }
}