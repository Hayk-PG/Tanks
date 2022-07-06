using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class BaseToggleCheck : MonoBehaviour
{
    protected Toggle _toggle;


    protected virtual void Awake()
    {
        _toggle = Get<Toggle>.From(gameObject);
    }

    private void Update()
    {
        _toggle.onValueChanged.RemoveAllListeners();
        _toggle.onValueChanged.AddListener(ToggleValueChanged);
    }

    public abstract void ToggleValueChanged(bool isOn);

    protected virtual void SimulateToggle(bool isOn)
    {
        if (isOn)
        {
            PointerEventData ped = new PointerEventData(EventSystem.current);
            _toggle.OnPointerClick(ped);
        }
    }
}
