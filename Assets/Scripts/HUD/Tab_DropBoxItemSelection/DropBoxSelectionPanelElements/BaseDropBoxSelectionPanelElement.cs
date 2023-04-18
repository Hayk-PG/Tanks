using UnityEngine;


public abstract class BaseDropBoxSelectionPanelElement : MonoBehaviour
{
    [SerializeField]
    protected Btn_DropBoxSelectionPanelElement _btnDropBoxSelectionPanelElement;

    [SerializeField] [Space]
    protected Texts_DropBoxSelectionPanelElement _textsDropBoxSelectionPanelElement;

    [SerializeField] [Space]
    protected string _title, _ability;

    [SerializeField] [Space]
    protected int _price, _quantity, _usageFrequency, _turns;
    protected int _usedTimes;

    protected bool _hasStartedUse;

    protected object[] _data = new object[10];

    protected virtual int NegativePrice
    {
        get
        {
            return _price > 0 ? -_price : _price;
        }
    }

    protected virtual bool CanUse { get; set; } = true;





    protected virtual void OnEnable() => _btnDropBoxSelectionPanelElement.Btn.onSelect += OnSelect;

    protected virtual void OnDisable() => _btnDropBoxSelectionPanelElement.Btn.onSelect -= OnSelect;

    public virtual void Activate()
    {
        if (!CanUse || !GameSceneObjectsReferences.DropBoxItemSelectionPanelOwner.HasEnoughPoints(_price))
        {
            gameObject.SetActive(false);

            return;
        }

        DisplayTitle();

        DisplayUsageFrequency();

        DisplayAbility();

        DisplayPrice();
    }

    protected virtual void OnSelect()
    {
        if (!CanUse)
            return;

        Use();

        CalculateUsedTimesAndCheckCanUseAgain();

        CloseTab();
    }

    protected abstract void Use();

    protected virtual void CloseTab() => GameSceneObjectsReferences.Tab_DropBoxItemSelection.SetActivity(false);

    protected virtual void CalculateUsedTimesAndCheckCanUseAgain()
    {
        if (!_hasStartedUse)
        {
            _usedTimes = _usageFrequency;

            _hasStartedUse = true;
        }

        _usedTimes--;

        if (_usedTimes <= 0)
            CanUse = false;
    }

    //For player's abilities
    public void Initialize(string title, string ability, int price, int quantity, int usageFrequency, int turns)
    {
        _title = title;
        _ability = ability;

        _price = price;
        _quantity = quantity;
        _usageFrequency = usageFrequency;
        _turns = turns;
    }

    public virtual void DisplayTitle() => _textsDropBoxSelectionPanelElement.Title.text = _title;

    public virtual void DisplayUsageFrequency() => _textsDropBoxSelectionPanelElement.UsageFrequency.text = UsageFrequencyText(_usageFrequency);

    public virtual void DisplayAbility() => _textsDropBoxSelectionPanelElement.Ability.text = _ability;

    public virtual void DisplayPrice() => SetPriceTagActive(_price > 0);

    public virtual void MakeAvailableAgain() => CanUse = true;

    protected virtual void SetPriceTagActive(bool isActive)
    {
        if (isActive)
        {
            _btnDropBoxSelectionPanelElement.BtnTxts[0].gameObject.SetActive(true);

            _btnDropBoxSelectionPanelElement.BtnTxts[1].gameObject.SetActive(false);

            _btnDropBoxSelectionPanelElement.Btn_IconStar.gameObject.SetActive(true);

            _btnDropBoxSelectionPanelElement.BtnTxts[0].SetButtonTitle(_price.ToString());
        }
        else
        {
            _btnDropBoxSelectionPanelElement.BtnTxts[0].gameObject.SetActive(false);

            _btnDropBoxSelectionPanelElement.BtnTxts[1].gameObject.SetActive(true);

            _btnDropBoxSelectionPanelElement.Btn_IconStar.gameObject.SetActive(false);

            _btnDropBoxSelectionPanelElement.BtnTxts[1].SetButtonTitle("Free");
        }
    }

    protected virtual string UsageFrequencyText(int timeUsage)
    {
        return timeUsage >= 100 ? "Can be used repeatedly throughout the game.": $"Can be used {TimeUsageText(timeUsage)} per game.";
    }

    protected virtual string TimeUsageText(int timeUsage)
    {
        return timeUsage == 1 ? "once" : timeUsage == 2 ? "twice" : $"{timeUsage} time";
    }
}
