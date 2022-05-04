using System;
using System.Collections.Generic;
using Common;
using Common.Interfaces;
using Common.ScriptableObjects;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class AppContext : MonoBehaviour, IAppContext
{
    [SerializeField] private LetterData[] _letters;
    [SerializeField] private CanvasInputSystem _inputSystem;
    [SerializeField] private float _drawingPrecision = 0.5f;

    [Space]
    [SerializeField] private Component _letterBrushContainer;
    [SerializeField] private Component _letterRendererContainer;

    private Dictionary<Type, object> _registeredTypes;

    private void Awake()
    {
        _registeredTypes = new Dictionary<Type, object>();

        RegisterInstance<LetterData[]>(_letters);
        RegisterInstance<IInputSystem>(_inputSystem);
        RegisterInstance<IDialogSystem>(new ConsoleDialogSystem());
        RegisterInstance<ILetterBuilder>(new LetterBuilder());
        RegisterInstance<ILetterSolver, ILetterEvents>(new LetterSolver(_drawingPrecision));
        RegisterInstance<IBrush>(_letterBrushContainer.GetComponent<IBrush>());
        RegisterInstance<ILetterRenderer>(_letterRendererContainer.GetComponent<ILetterRenderer>());
    }

    public T Resolve<T>()
    {
        return (T) _registeredTypes[typeof(T)];
    }

    private void RegisterInstance<T>(T instance)
    {
        _registeredTypes.Add(typeof(T), instance);
    }

    private void RegisterInstance<T1, T2>(object instance)
    {
        _registeredTypes.Add(typeof(T1), instance);
        _registeredTypes.Add(typeof(T2), instance);
    }
}