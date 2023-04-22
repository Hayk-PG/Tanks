using UnityEngine;
using UnityEngine.UI;

public class SliderBar_Sound : Button_Sound
{
    private Slider _slider;
    private float _previousValue;
    private float _currentValue;


    private void Awake()
    {
        _slider = Get<Slider>.From(gameObject);
    }

    public void OnValueChanged()
    {
        _previousValue = _slider.value;

        if(_currentValue + 5 <= _previousValue)
        {
            PlaySound();           
            _currentValue = _previousValue;
        }
    }

    private void PlaySound()
    {
        UISoundController.PlaySound(_listIndex, _clipIndex);
    }
}
