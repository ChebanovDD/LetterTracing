using Common.Interfaces;
using Common.ScriptableObjects;
using UnityEngine;

namespace SpriteRendererImplementation
{
    public class SpriteLetterRenderer : MonoBehaviour, ILetterRenderer
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public void DrawLetter(LetterData letterData)
        {
            _spriteRenderer.sprite = Resources.Load<Sprite>($"Sprites/{letterData.Name}");
        }

        public void Clear()
        {
            var sprite = _spriteRenderer.sprite;
            if (sprite == null)
            {
                return;
            }

            _spriteRenderer.sprite = null;
            Resources.UnloadAsset(sprite);
        }

        private void OnDestroy()
        {
            Clear();
        }
    }
}