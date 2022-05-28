﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class BaseSelectTankButton : MonoBehaviour, ISelectTankButton<BaseSelectTankButton, Button>
{
    [SerializeField] protected Image _iconTank;
    [SerializeField] protected Sprite[] _buttonSprites;
    [SerializeField] protected Text _textTankName;
    [SerializeField] protected Text _textPlayerLevel;
    [SerializeField] protected Stars _stars;
    protected Transform _parent;   
    protected Data _data;
    protected int _index;
    public BaseSelectTankButton[] SelectTankButtons { get; set; }
    public Button Button { get; set; }


    protected virtual void Awake()
    {
        _index = transform.GetSiblingIndex();
        _parent = transform.parent;        
        _data = FindObjectOfType<Data>();
        SelectTankButtons = _parent.GetComponentsInChildren<BaseSelectTankButton>();
        Button = Get<Button>.From(gameObject);
    }

    protected virtual void Start()
    {
        SetIcon();
        InitializeTankStats();
    }

    protected virtual void Update()
    {
        Button?.onClick.RemoveAllListeners();
        Button.onClick.AddListener(OnClickTankButton);
    }
   
    protected virtual void SetIcon()
    {
        if (IsIndexCorrect()) _iconTank.sprite = _data.AvailableTanks[_index]._iconTank;
    }
   
    public void ButtonSprite(bool isSelected)
    {
        if (isSelected)
            Button.image.sprite = _buttonSprites[0];
        else
            Button.image.sprite = _buttonSprites[1];
    }

    protected virtual void DeselectAllButtonsAndSelectThis()
    {
        foreach (var tankButton in SelectTankButtons)
        {
            if (tankButton == this)
                ButtonSprite(true);
            else
                tankButton.ButtonSprite(false);
        }
    }

    protected virtual void SimulateButtonClick()
    {
        if (CanAutoClick())
        {
            PointerEventData ped = new PointerEventData(EventSystem.current);
            Button.OnPointerClick(ped);
            Button.OnSubmit(ped);
            transform.SetAsFirstSibling();
            DisplayTankInfo();
        }
    }

    public virtual void OnClickTankButton()
    {
        if (IsIndexCorrect())
        {
            DeselectAllButtonsAndSelectThis();
            SetData();
            DisplayTankInfo();
        }
    }

    protected abstract bool IsIndexCorrect();
    protected abstract bool CanAutoClick();
    protected abstract void SetData();
    protected abstract void InitializeTankStats();
    protected abstract void DisplayTankInfo();
}
