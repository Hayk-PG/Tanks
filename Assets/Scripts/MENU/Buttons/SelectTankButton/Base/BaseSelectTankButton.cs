using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class BaseSelectTankButton : MonoBehaviour, ISelectTankButton<BaseSelectTankButton, Button>
{
    [SerializeField] internal Image _iconTank;
    [SerializeField] internal Image _iconArrow;
    [SerializeField] internal Sprite[] _buttonSprites;
    [SerializeField] internal Text _textTankName;
    [SerializeField] internal Text _textPlayerLevel;
    [SerializeField] internal Stars _stars;
    [SerializeField] internal GameObject _lockGameObject;

    internal Data _data;
    internal TanksInfo _tanksInfo;
    internal BaseInitializeTankButton _baseInitializeTankButton;
    internal BaseTankButtonData _baseTankButtonData;
    internal BaseTankButtonInfo _baseTankButtonInfo;

    internal Transform _parent;
    internal int _index;
    public bool IsLocked
    {
        get => _lockGameObject.activeInHierarchy;
        set => _lockGameObject.SetActive(value);
    }
    public bool IsClicked
    {
        get => _iconArrow.gameObject.activeInHierarchy;
        set => _iconArrow.gameObject.SetActive(value);
    }
    public BaseSelectTankButton[] SelectTankButtons { get; set; }
    public Button Button { get; set; }


    protected virtual void Awake()
    {
        _index = transform.GetSiblingIndex();
        Button = Get<Button>.From(gameObject);
        _parent = transform.parent;

        SelectTankButtons = _parent.GetComponentsInChildren<BaseSelectTankButton>();
        _data = FindObjectOfType<Data>();
        _tanksInfo = Get<TanksInfo>.From(gameObject);
        _baseInitializeTankButton = Get<BaseInitializeTankButton>.From(gameObject);
        _baseTankButtonData = Get<BaseTankButtonData>.From(gameObject);
        _baseTankButtonInfo = Get<BaseTankButtonInfo>.From(gameObject);
    }

    protected virtual void Start()
    {
        if (IsIndexCorrect())
        {
            _baseInitializeTankButton.Properties(this);
            _baseInitializeTankButton.Icon(this);
        }
    }

    protected virtual void Update()
    {
        Button?.onClick.RemoveAllListeners();
        Button.onClick.AddListener(OnClickTankButton);
    }

    public virtual void OnClickTankButton()
    {
        if (IsIndexCorrect())
        {
            if (!MyPhotonNetwork.IsOfflineMode) Conditions<bool>.Compare(IsLocked, OnLocked, OnUnlocked);
            if (MyPhotonNetwork.IsOfflineMode) OnUnlocked();
            ClickedIndicator();
        }
    }

    protected virtual void OnLocked()
    {
        _baseTankButtonInfo.TankNotOwnedScreen(this);
        _baseTankButtonInfo.RequiredItemsScreen(this);
    }

    protected virtual void OnUnlocked()
    {
        _baseTankButtonData.Save(this);
        _baseTankButtonInfo.TankOwnedScreen(this);
        DeselectAllButtonsAndSelectThis();
    }

    public void ButtonSprite(bool isSelected, bool isLocked)
    {
        if (isSelected)
        {
            if (!isLocked) Button.image.sprite = _buttonSprites[0];
            if (isLocked) Button.image.sprite = _buttonSprites[2];          
        }
        else
        {
            if (!isLocked) Button.image.sprite = _buttonSprites[1];
            if (isLocked) Button.image.sprite = _buttonSprites[2];
        }
    }

    protected void ClickedIndicator()
    {
        foreach (var tankButton in SelectTankButtons)
        {
            if (tankButton == this) IsClicked = true;
            if (tankButton != this) tankButton.IsClicked = false;
        }
    }

    protected virtual void DeselectAllButtonsAndSelectThis()
    {
        foreach (var tankButton in SelectTankButtons)
        {
            if (tankButton == this) ButtonSprite(true, IsLocked);
            if (tankButton != this) tankButton.ButtonSprite(false, tankButton.IsLocked);
        }
    }

    protected virtual void SimulateButtonClick()
    {
        if (CanAutoClick())
        {
            PointerEventData ped = new PointerEventData(EventSystem.current);
            Button.OnSubmit(ped);
            transform.SetAsFirstSibling();
        }        
    }

    protected virtual bool IsIndexCorrect()
    {
        return false;
    }

    protected virtual bool CanAutoClick()
    {
        return false;
    } 
}
