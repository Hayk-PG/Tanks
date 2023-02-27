using UnityEngine;


public abstract class BaseDropBoxSelectionPanelElement : MonoBehaviour
{
    [SerializeField]
    protected Btn_DropBoxSelectionPanelElement _btnDropBoxSelectionPanelElement;

    [SerializeField] [Space]
    protected int _price, _quantity;

    protected virtual bool CanUse { get; set; } = true;



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
        if (!CanUse || !GameSceneObjectsReferences.DropBoxItemSelectionPanelOwner.HasEnoughPoints(_price))
        {
            gameObject.SetActive(false);

            return;
        }

        DisplayPrice();

        DisplayQuantity();
    }

    protected virtual void OnSelect()
    {
        if (!CanUse)
            return;

        Use();

        CloseTab();
    }

    protected abstract void Use();

    protected virtual void CloseTab() => GameSceneObjectsReferences.Tab_DropBoxItemSelection.SetActivity(false);

    protected virtual void DisplayPrice()
    {
        if(_price == 0)
        {
            _btnDropBoxSelectionPanelElement.BtnTxts[0].gameObject.SetActive(false);

            return;
        }

        _btnDropBoxSelectionPanelElement.BtnTxts[0].gameObject.SetActive(true);
        _btnDropBoxSelectionPanelElement.BtnTxts[0].SetButtonTitle(_price.ToString());
    }

    protected virtual void DisplayQuantity(string txt = "")
    {
        if(_quantity == 0)
        {
            _btnDropBoxSelectionPanelElement.BtnTxts[1].gameObject.SetActive(false);

            return;
        }

        _btnDropBoxSelectionPanelElement.BtnTxts[1].gameObject.SetActive(true);
        _btnDropBoxSelectionPanelElement.BtnTxts[1].SetButtonTitle(_quantity + txt);
    }

    public virtual void MakeAvailableAgain()
    {
        CanUse = true;
    }
}
