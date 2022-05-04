using Common.Models;
using UnityEngine;

namespace Common.Interfaces
{
    public interface ILetterSolver : ILetterEvents
    {
        public bool InProcess { get; }
        public bool IsLetterSolved { get; }

        void SetLetter(Letter letter);
        bool IsStartPoint(Vector2 mousePosition);
        void Start(Vector2 mousePosition);
        void Calculate(Vector2 mousePosition);
        void Stop();
    }
}