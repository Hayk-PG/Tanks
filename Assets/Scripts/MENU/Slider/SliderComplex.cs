using System;
using UnityEngine;
using UnityEngine.UI;

public class SliderComplex : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    public event Action<float> onSendValue;


    private void Update()
    {
        _slider.onValueChanged.RemoveAllListeners();
        _slider.onValueChanged.AddListener(GetValue);
    }

    private void GetValue(float value)
    {
        onSendValue?.Invoke(value);
    }

    public void SetValue(float value)
    {
        _slider.value = value;
    }
}
