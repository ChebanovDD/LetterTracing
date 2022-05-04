using System;
using UnityEngine;

namespace Common.Interfaces
{
    public interface IInputSystem
    {
        event EventHandler<Vector2> MouseDown;
        event EventHandler<Vector2> MouseMove;
        event EventHandler<Vector2> MouseUp;
    }
}