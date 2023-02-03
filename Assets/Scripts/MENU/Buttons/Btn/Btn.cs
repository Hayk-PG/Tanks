using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]

public class Btn : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public enum ButtonClickType { ChangeSprite, ChangeColor, Both, None, OnlyInvokeEvent}
    public ButtonClickType _buttonClickType;

    private Btn[] _siblings;

    [SerializeField] private Sprite _sprtPressed;
    private Sprite _sprtReleased;

    [SerializeField] private Color _clrPressed;
    private Color _clrReleased;

    private Button Button { get; set; }
    private Sprite ButtonSprite
    {
        get => Button.image.sprite;
        set => Button.image.sprite = value;
    }
    private Color ButtonColor
    {
        get => Button.image.color;
        set => Button.image.color = value;
    }

    public bool IsInteractable
    {
        get => Button.interactable;
        set => Button.interactable = value;
    }
    public bool IsSelected { get; private set; }

    public event Action onSelect;
    public event Action onDeselect;
    public event Action onPointerDown;
    public event Action onPointerUp;




    private void Awake()
    {
        GetButton();
        CacheButtonDefaultLook();
    }

    private void Start()
    {
        GetSiblings();

        Button.onClick.AddListener(Select);
    }

    private void GetButton()
    {
        Button = Get<Button>.From(gameObject);
    }

    private void CacheButtonDefaultLook()
    {
        _sprtReleased = Button.image.sprite;
        _clrReleased = Button.image.color;
    }

    private void GetSiblings()
    {
        _siblings = new Btn[transform.parent.childCount - 1];

        for (int i = 0, j = 0; i < transform.parent.childCount; i++)
        {
            if (Get<Btn>.From(transform.parent.GetChild(i).gameObject) != this)
            {
                _siblings[j] = Get<Btn>.From(transform.parent.GetChild(i).gameObject);
                j++;
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData) => onPointerDown?.Invoke();

    public void OnPointerDown(PointerEventData eventData) => onPointerUp?.Invoke();

    public void Select()
    {
        if (_buttonClickType == ButtonClickType.None)
            return;

        if (!IsSelected)
        {
            IsSelected = true;
            onSelect?.Invoke();

            ChangeBuutonLook(_sprtPressed, _clrPressed);
            Conditions<bool>.Compare(_buttonClickType == ButtonClickType.OnlyInvokeEvent, Deselect, DeselectAllSiblings);
        }
    }

    private void DeselectAllSiblings()
    {
        GlobalFunctions.Loop<Btn>.Foreach(_siblings, sibling =>
        {
            sibling?.Deselect();
        });
    }

    public void Deselect()
    {
        if (IsSelected)
        {
            IsSelected = false;
            onDeselect?.Invoke();

            if (_sprtReleased == null)
                return;

            ChangeBuutonLook(_sprtReleased, _clrReleased);
        }
    }

    private void ChangeBuutonLook(Sprite sprite, Color color)
    {
        switch (_buttonClickType)
        {
            case ButtonClickType.ChangeSprite: ChangeSprite(sprite); break;
            case ButtonClickType.ChangeColor: ChangeColor(color); break;
            case ButtonClickType.Both: ChangeBoth(sprite, color); break;
        }
    }

    private void ChangeSprite(Sprite sprite)
    {
        if (_sprtPressed == null)
            return;

        ButtonSprite = sprite;
    }

    private void ChangeColor(Color color)
    {
        ButtonColor = color;
    }

    private void ChangeBoth(Sprite sprite, Color color)
    {
        if (_sprtPressed == null)
            return;

        ButtonSprite = sprite;
        ButtonColor = color;
    }
}
