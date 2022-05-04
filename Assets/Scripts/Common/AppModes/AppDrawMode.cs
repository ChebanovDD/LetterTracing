using System;
using Common.Interfaces;
using Common.Models;
using Common.ScriptableObjects;
using UnityEngine;

namespace Common.AppModes
{
    public class AppDrawMode : IAppMode
    {
        private readonly IBrush _brush;
        private readonly LetterData[] _letters;
        private readonly IInputSystem _inputSystem;
        private readonly IDialogSystem _dialogSystem;
        private readonly ILetterSolver _letterSolver;
        private readonly ILetterBuilder _letterBuilder;
        private readonly ILetterRenderer _letterRenderer;

        private int _activeLetterIndex;

        public AppDrawMode(IAppContext appContext)
        {
            _brush = appContext.Resolve<IBrush>();
            _letters = appContext.Resolve<LetterData[]>();
            _inputSystem = appContext.Resolve<IInputSystem>();
            _dialogSystem = appContext.Resolve<IDialogSystem>();
            _letterSolver = appContext.Resolve<ILetterSolver>();
            _letterBuilder = appContext.Resolve<ILetterBuilder>();
            _letterRenderer = appContext.Resolve<ILetterRenderer>();
        }

        public event EventHandler Finished;

        public void Activate()
        {
            _inputSystem.MouseDown += OnCanvasMouseDown;
            _inputSystem.MouseMove += OnCanvasMouseMove;
            _inputSystem.MouseUp += OnCanvasMouseUp;

            _letterSolver.StrokeCompleted += OnStrokeCompleted;
            _letterSolver.StrokeFailed += OnStrokeFailed;

            ActivateNextLetter();
        }

        public void Deactivate()
        {
            _inputSystem.MouseDown -= OnCanvasMouseDown;
            _inputSystem.MouseMove -= OnCanvasMouseMove;
            _inputSystem.MouseUp -= OnCanvasMouseUp;

            _letterSolver.StrokeCompleted -= OnStrokeCompleted;
            _letterSolver.StrokeFailed -= OnStrokeFailed;
        }

        private void ActivateNextLetter()
        {
            if (_activeLetterIndex >= _letters.Length)
            {
                Finished?.Invoke(this, EventArgs.Empty);
                return;
            }

            _brush.Clear();
            _letterRenderer.Clear();
            _letterRenderer.DrawLetter(_letters[_activeLetterIndex]);
            _letterSolver.SetLetter(_letterBuilder.BuildLetter(_letters[_activeLetterIndex]));
            _activeLetterIndex++;
        }

        private void OnCanvasMouseDown(object sender, Vector2 mousePosition)
        {
            if (_letterSolver.IsLetterSolved)
            {
                return;
            }

            if (_letterSolver.IsStartPoint(mousePosition))
            {
                _letterSolver.Start(mousePosition);
            }
            else
            {
                _dialogSystem.ShowMessage("Wrong start point.");
            }
        }

        private void OnCanvasMouseMove(object sender, Vector2 mousePosition)
        {
            if (_letterSolver.InProcess)
            {
                _brush.Draw(mousePosition);
                _letterSolver.Calculate(mousePosition);
            }
        }

        private void OnCanvasMouseUp(object sender, Vector2 mousePosition)
        {
            if (_letterSolver.IsLetterSolved)
            {
                ActivateNextLetter();
            }
            else
            {
                _letterSolver.Stop();
            }
        }

        private void OnStrokeCompleted(object sender, Stroke stroke)
        {
            if (_letterSolver.IsLetterSolved == false)
            {
                _dialogSystem.ShowMessage("Stroke completed.");
            }
        }

        private void OnStrokeFailed(object sender, Stroke stroke)
        {
            _dialogSystem.ShowMessage("Fail.");
        }
    }
}