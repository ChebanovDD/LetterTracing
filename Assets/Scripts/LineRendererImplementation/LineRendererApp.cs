using Common;
using Common.Interfaces;
using UnityEngine;

namespace LineRendererImplementation
{
    public class LineRendererApp : App
    {
        [SerializeField] private LineLetterRenderer _lineLetterRenderer;
        
        protected override ILetterRenderer GetLetterRenderer(ILetterBuilder letterBuilder)
        {
            return _lineLetterRenderer.Construct(letterBuilder);
        }
    }
}