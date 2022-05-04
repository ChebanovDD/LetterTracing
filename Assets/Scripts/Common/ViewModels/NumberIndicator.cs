using TMPro;
using UnityEngine;

namespace Common.ViewModels
{
    public class NumberIndicator : MonoBehaviour
    {
        [SerializeField] private TMP_Text _label;

        public void SetNumber(int number)
        {
            _label.text = number.ToString();
        }
    }
}