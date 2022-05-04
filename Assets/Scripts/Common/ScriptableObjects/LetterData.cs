using Common.Models;
using UnityEngine;

namespace Common.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewLetter", menuName = "Letter Data")]
    public class LetterData : ScriptableObject
    {
        public string Name;
        public StrokeData[] Strokes;
    }
}