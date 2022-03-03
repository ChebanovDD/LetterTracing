using Common.Interfaces;
using UnityEngine;

namespace LineRendererImplementation
{
    public class LineLetterRenderer : MonoBehaviour, ILetterRenderer
    {
        private ILetterBuilder _letterBuilder;
        
        public ILetterRenderer Construct(ILetterBuilder letterBuilder)
        {
            _letterBuilder = letterBuilder;
            
            return this;
        }
    }
}