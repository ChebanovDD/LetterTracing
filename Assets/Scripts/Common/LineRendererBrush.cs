using Common.Interfaces;
using UnityEngine;

namespace Common
{
    public class LineRendererBrush : MonoBehaviour, IBrush
    {
        private ILetterSolver _letterSolver;
        
        public IBrush Construct(ILetterSolver letterSolver)
        {
            _letterSolver = letterSolver;
            
            return this;
        }
    }
}