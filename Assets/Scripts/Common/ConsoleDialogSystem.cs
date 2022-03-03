using Common.Interfaces;
using UnityEngine;

namespace Common
{
    public class ConsoleDialogSystem : IDialogSystem
    {
        public void ShowMessage(string message)
        {
            Debug.Log(message);
        }
    }
}