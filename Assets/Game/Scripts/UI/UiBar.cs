using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    public class UiBar : MonoBehaviour
    {
        [SerializeField] private Slider _slider;

        public Slider Slider => _slider;

        public void Initialize(int maxValue)
        {
            _slider.maxValue = maxValue;
        }

        public void IncreaseValue(int value)
        {
            _slider.value += value;
        }

        public void SetValue(int value)
        {
            _slider.value = value;
        }
    }
}
