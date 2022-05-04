using Common.ScriptableObjects;

namespace Common.Interfaces
{
    public interface ILetterRenderer
    {
        void DrawLetter(LetterData letterData);
        void Clear();
    }
}