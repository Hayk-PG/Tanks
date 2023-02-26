using UnityEngine;


public abstract class BaseDropBoxSelectionPanelElement : MonoBehaviour
{
    [SerializeField]
    protected Btn_DropBoxSelectionPanelElement _btnDropBoxSelectionPanelElement;

    [SerializeField] [Space]
    private int _price;

    protected virtual bool CanUse { get; set; }



    protected virtual void OnEnable()
    {
        _btnDropBoxSelectionPanelElement.Btn.onSelect += OnSelect;
    }

    protected virtual void OnDisable()
    {
        _btnDropBoxSelectionPanelElement.Btn.onSelect -= OnSelect;
    }

    public virtual void Activate()
    {

    }

    protected virtual void OnSelect()
    {
        if (!CanUse)
            return;

        Use();
    }

    protected abstract void Use();
}
