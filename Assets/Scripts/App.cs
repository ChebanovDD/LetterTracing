using System;
using Common.AppModes;
using Common.Interfaces;
using UnityEngine;

public class App : MonoBehaviour
{
    [SerializeField] private AppContext _appContext;

    private IAppMode _drawLettersMode;
    private IDialogSystem _dialogSystem;

    private void Awake()
    {
        _drawLettersMode = new AppDrawMode(_appContext);
        _dialogSystem = _appContext.Resolve<IDialogSystem>();
    }

    private void OnEnable()
    {
        _drawLettersMode.Finished += OnDrawLettersModeFinished;
    }

    private void Start()
    {
        _drawLettersMode.Activate();
    }

    private void OnDisable()
    {
        _drawLettersMode.Finished -= OnDrawLettersModeFinished;
    }

    private void OnDrawLettersModeFinished(object sender, EventArgs e)
    {
        _drawLettersMode.Deactivate();
        _dialogSystem.ShowMessage("Well done.");
    }
}