using UnityEngine;
using UnityEngine.UI;

public class GlobalOnButtonInteract : MonoBehaviour
{    
    public enum ButtonState { Released, Clicked}

    [HideInInspector]
    public ButtonState _buttonState;

    private Image _buttonImage;

    [SerializeField]
    [Tooltip("0: Released 1: Clicked")]
    private Sprite[] _sprites;

    [SerializeField]
    [Tooltip("0: Released 1: Clicked")]
    private Color[] _colors;

    private Sprite ButtonSprite
    {
        get => _buttonImage.sprite;
        set => _buttonImage.sprite = value;
    }
    private Color ButtonColor
    {
        get => _buttonImage.color;
        set => _buttonImage.color = value;
    }


    private void Awake()
    {
        IOnButtonInteract onButtonInteract = Get<IOnButtonInteract>.From(gameObject);
        if (onButtonInteract != null) onButtonInteract.GlobalOnButtonInteract = this;
    }

    public void OnChangeButtonSprite(ButtonState buttonState)
    {
        if (_buttonImage == null) _buttonImage = Get<Image>.From(gameObject);

        switch (buttonState)
        {
            case ButtonState.Released: ButtonSprite = _sprites[0]; break;
            case ButtonState.Clicked: ButtonSprite = _sprites[1]; break;
        }
    }

    public void OnChangeButtonColor(ButtonState buttonState)
    {
        if (_buttonImage == null) _buttonImage = Get<Image>.From(gameObject);

        switch (buttonState)
        {
            case ButtonState.Released: ButtonColor = _colors[0]; break;
            case ButtonState.Clicked: ButtonColor = _colors[1]; break;
        }
    }

    public void OnChangeButtonSpriteAndColor(ButtonState buttonState)
    {
        if (_buttonImage == null) _buttonImage = Get<Image>.From(gameObject);

        switch (buttonState)
        {
            case ButtonState.Released:
                ButtonSprite = _sprites[0];
                ButtonColor = _colors[0]; break;
            case ButtonState.Clicked:
                ButtonSprite = _sprites[1];
                ButtonColor = _colors[1]; break;
        }
    }
}
