using System;
using Common.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Common
{
    public class CanvasInputSystem : MonoBehaviour, IInputSystem
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private EventTrigger _eventTrigger;

        public event EventHandler<Vector2> MouseDown;
        public event EventHandler<Vector2> MouseMove;
        public event EventHandler<Vector2> MouseUp;

        private void Awake()
        {
            var pointerDown = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
            pointerDown.callback.AddListener(data => { OnPointerDown((PointerEventData) data); });

            var drag = new EventTrigger.Entry { eventID = EventTriggerType.Drag };
            drag.callback.AddListener(data => { OnPointerDrag((PointerEventData) data); });

            var pointerUp = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
            pointerUp.callback.AddListener(data => { OnPointerUp((PointerEventData) data); });

            _eventTrigger.triggers.Add(pointerDown);
            _eventTrigger.triggers.Add(drag);
            _eventTrigger.triggers.Add(pointerUp);
        }

        private void OnPointerDown(PointerEventData e)
        {
            MouseDown?.Invoke(this, GetWorldPosition(e.position));
        }

        private void OnPointerDrag(PointerEventData e)
        {
            MouseMove?.Invoke(this, GetWorldPosition(e.position));
        }

        private void OnPointerUp(PointerEventData e)
        {
            MouseUp?.Invoke(this, GetWorldPosition(e.position));
        }

        private Vector2 GetWorldPosition(Vector2 screenPosition)
        {
            return _camera.ScreenToWorldPoint(screenPosition);
        }
    }
}