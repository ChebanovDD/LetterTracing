using Common.Models;
using Common.ScriptableObjects;

namespace Common.Interfaces
{
    public interface ILetterBuilder
    {
        Letter BuildLetter(LetterData letterData);
        Segment[] BuildStrokeSegments(StrokeData strokeData);
    }
}