using System;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Button))]
public class Btn : MonoBehaviour, IReset
{
    private enum ButtonClickType { ChangeSprite, ChangeColor, Both}

    [SerializeField] private ButtonClickType _buttonClickType;

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

    public event Action _onClick;


    private void Awake()
    {
        GetButton();
        CacheButtonDefaultLook();
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

    public void Click()
    {
        _onClick?.Invoke();

        switch (_buttonClickType)
        {
            case ButtonClickType.ChangeSprite: ChangeSprite(); break;
            case ButtonClickType.ChangeColor: ChangeColor(); break;
            case ButtonClickType.Both: ChangeBoth(); break;
        }
    }

    public void ChangeSprite()
    {
        if (_sprtPressed == null)
            return;

        ButtonSprite = _sprtPressed;
    }

    public void ChangeColor()
    {
        ButtonColor = _clrPressed;
    }

    public void ChangeBoth()
    {
        if (_sprtPressed == null)
            return;

        ButtonSprite = _sprtPressed;
        ButtonColor = _clrPressed;
    }

    public void SetDefault()
    {
        if (_sprtReleased == null)
            return;

        ButtonSprite = _sprtReleased;
        ButtonColor = _clrReleased;
    }
}
