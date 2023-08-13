using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    public class UiBar : MonoBehaviour
    {
        [SerializeField] private Slider          _slider;
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _values;


        private Func<int, string> get_values_func = null;
        private Func<int, string> get_title_func = null;

        public Slider Slider => _slider;

        private void Awake()
        {
            _slider.onValueChanged.AddListener( updateTitleText );
            _slider.onValueChanged.AddListener( updateValuesText );
        }

        public void Initialize(int maxValue, Func<int, string> get_title_func = null, Func<int, string> get_values_func = null )
        {
            _slider.maxValue = maxValue;
            this.get_title_func = get_title_func;
            this.get_values_func = get_values_func;
        }

        public void IncreaseValue(int value)
        {
            _slider.value += value;
        }

        public void SetValue(int value)
        {
            _slider.value = value;
        }

        private void updateTitleText(float val)
        {
            if ( !_title || get_title_func == null )
                return;

            _title.text = get_title_func( (int)val );
        }

        private void updateValuesText(float val)
        {
            if ( !_values )
                return;

            _values.text = get_values_func != null ? get_values_func( (int)val ) : $"{(int)val}/{Slider.maxValue}";
        }
    }
}
