using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class BaseSliderLevel<T> : MonoBehaviour, IReset
{
    [SerializeField]
    protected Slider _slider;
    [SerializeField]
    protected TMP_Text _title;

    protected int SliderValue { get => Mathf.FloorToInt(_slider.value); set => _slider.value = value; }

    protected abstract string Title(string suffix);
    protected abstract string Result(T t);
    protected abstract void UpdateTitleText(T t);  
    public abstract void OnSliderValueChanged();
    public virtual void SetDefault()
    {
        
    }
}
