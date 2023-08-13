using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    public class UiBar : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private TextMeshProUGUI _text;


        private Func<int, string> get_text_func = null;

        public Slider Slider => _slider;

        private void Awake()
        {
            _slider.onValueChanged.AddListener( val =>
            {
                if ( !_text || get_text_func == null )
                    return;

                _text.text = get_text_func( (int)val );
                return;
            } );
        }

        public void Initialize(int maxValue, Func<int, string> get_text_func = null)
        {
            _slider.maxValue = maxValue;
            this.get_text_func = get_text_func;
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
