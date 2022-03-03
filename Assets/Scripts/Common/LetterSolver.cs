using Common.Interfaces;

namespace Common
{
    public class LetterSolver : ILetterSolver
    {
        private readonly float _drawingPrecision;
        
        public LetterSolver(float drawingPrecision)
        {
            _drawingPrecision = drawingPrecision;
        }
    }
}