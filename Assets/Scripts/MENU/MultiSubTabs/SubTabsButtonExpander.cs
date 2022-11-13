using UnityEngine;

[RequireComponent(typeof(SubTabsButton))]

public abstract class SubTabsButtonExpander : MonoBehaviour
{
    protected SubTabsButton _subTabBtn;


    protected virtual void Awake()
    {
        _subTabBtn = Get<SubTabsButton>.From(gameObject);
    }

    protected virtual void OnEnable()
    {
        _subTabBtn.onSelect += Click;
    }

    protected virtual void OnDisable()
    {
        _subTabBtn.onSelect -= Click;
    }

    protected abstract void Click();
}
