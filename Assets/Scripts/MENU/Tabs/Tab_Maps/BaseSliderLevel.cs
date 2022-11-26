using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class BaseSliderLevel<T> : MonoBehaviour, IReset
{
    protected Slider _slider;
    protected TMP_Text _title;

    protected int SliderValue { get => Mathf.FloorToInt(_slider.value); set => _slider.value = value; }


    protected virtual void Awake()
    {
        _slider = Get<Slider>.FromChild(gameObject);
        _title = Get<TMP_Text>.FromChild(gameObject);
    }

    protected virtual void Update()
    {
        _slider.onValueChanged.RemoveAllListeners();
        _slider.onValueChanged.AddListener(delegate { ListenSliderValueChange(); });
    }

    protected abstract void ListenSliderValueChange();
    protected abstract string Title(string suffix);
    protected abstract string Result(T t);
    protected abstract void UpdateTitleText(T t);  
    
    public virtual void SetDefault()
    {
        
    }
}
