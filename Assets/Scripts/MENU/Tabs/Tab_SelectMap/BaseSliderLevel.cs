using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class BaseSliderLevel<T> : MonoBehaviour
{
    [SerializeField]
    protected Slider _slider;
    [SerializeField]
    protected TMP_Text _title;

    protected CanvasGroup _canvasGroup;
    protected Tab_SelectMaps _tabSelectMaps;


    protected virtual void Awake()
    {
        _canvasGroup = Get<CanvasGroup>.From(gameObject);
        _tabSelectMaps = FindObjectOfType<Tab_SelectMaps>();
    }

    protected virtual void OnEnable()
    {
        _tabSelectMaps.OnTabOpened += OnTabOpened;
    }

    protected virtual void OnDisable()
    {
        _tabSelectMaps.OnTabOpened -= OnTabOpened;
    }

    protected abstract string Title(string suffix);
    protected abstract void UpdateTitleText(T t);
    protected abstract void OnTabOpened();
    public abstract void OnSliderValueChanged();
}
