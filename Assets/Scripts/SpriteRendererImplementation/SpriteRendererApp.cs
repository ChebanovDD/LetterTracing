using Common;
using Common.Interfaces;
using UnityEngine;

namespace SpriteRendererImplementation
{
    public class SpriteRendererApp : App
    {
        [SerializeField] private SpriteLetterRenderer _spriteLetterRenderer;
        
        protected override ILetterRenderer GetLetterRenderer(ILetterBuilder letterBuilder)
        {
            return _spriteLetterRenderer;
        }
    }
}