using System;
using Common.Interfaces;
using UnityEngine;

namespace Common
{
    public abstract class App : MonoBehaviour
    {
        [SerializeField] private float _drawingPrecision = 0.5f;
        [SerializeField] private CanvasInputSystem _canvasInputSystem;
        
        [Space]
        [SerializeField] private LineRendererBrush _lineRendererBrush;
        
        private IBrush _brush;
        private ILetterSolver _letterSolver;
        private ILetterBuilder _letterBuilder;
        private ILetterRenderer _letterRenderer;

        private IInputSystem _inputSystem;
        private IDialogSystem _dialogSystem;
        
        private void Awake()
        {
            Initialize();
        }

        private void OnEnable()
        {
            SubscribeOnEvents();
        }

        private void Start()
        {
            ActivateNextLetter();
        }

        private void OnDisable()
        {
            UnsubscribeFromEvents();
        }

        private void Initialize()
        {
            _letterSolver = new LetterSolver(_drawingPrecision);
            _letterBuilder = new LetterBuilder();
            _dialogSystem = new ConsoleDialogSystem();

            _brush = GetBrush(_letterSolver);
            _inputSystem = GetInputSystem();
            _letterRenderer = GetLetterRenderer(_letterBuilder);
        }

        private IBrush GetBrush(ILetterSolver letterSolver)
        {
            return _lineRendererBrush.Construct(letterSolver);
        }

        private IInputSystem GetInputSystem()
        {
            return _canvasInputSystem;
        }

        protected abstract ILetterRenderer GetLetterRenderer(ILetterBuilder letterBuilder);
        
        private void SubscribeOnEvents()
        {
            _inputSystem.MouseDown += OnMouseDown;
            _inputSystem.MouseDrag += OnMouseDrag;
            _inputSystem.MouseUp += OnMouseUp;
        }

        private void UnsubscribeFromEvents()
        {
            _inputSystem.MouseDown -= OnMouseDown;
            _inputSystem.MouseDrag -= OnMouseDrag;
            _inputSystem.MouseUp -= OnMouseUp;
        }

        private void ActivateNextLetter()
        {
        }
        
        private void OnMouseDown(object sender, Vector2 mouseWorldPosition)
        {
            throw new NotImplementedException();
        }

        private void OnMouseDrag(object sender, Vector2 mouseWorldPosition)
        {
            throw new NotImplementedException();
        }

        private void OnMouseUp(object sender, Vector2 mouseWorldPosition)
        {
            throw new NotImplementedException();
        }
    }
}