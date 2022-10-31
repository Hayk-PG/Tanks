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
    protected SubTab _subTab;


    protected virtual void Awake()
    {
        _canvasGroup = Get<CanvasGroup>.From(gameObject);
        _subTab = Get<SubTab>.From(gameObject);
    }

    protected virtual void OnEnable()
    {
        if (_subTab == null)
            return;

        _subTab._onActivity += GetTabActivity;
    }

    protected virtual void OnDisable()
    {
        if (_subTab == null)
            return;

        _subTab._onActivity -= GetTabActivity;
    }

    protected virtual void GetTabActivity(bool isActive)
    {
        Conditions<bool>.Compare(isActive, Activate, null);
    }

    protected abstract void Activate();
    protected abstract string Title(string suffix);
    protected abstract void UpdateTitleText(T t);  
    public abstract void OnSliderValueChanged();
}
