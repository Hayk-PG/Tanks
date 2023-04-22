using System;
using UnityEngine;
using UnityEngine.UI;

public class WeaponsTabButton : MonoBehaviour
{
    private Image _image;

    private Text _buttonText;

    public Color Color
    {
        get => _image.color;
        set => _image.color = value;
    }
    public string ButtonName
    {
        get => _buttonText.text;
        set => _buttonText.text = value;
    }
    public CanvasGroup RelatedContainer { get; set; }

    public event Action<WeaponsTabButton> OnWeaponsTabButtonClicked;



    private void Awake()
    {
        _image = GetComponent<Image>();

        _buttonText = Get<Text>.FromChild(gameObject);
    }

    public void OnClick() => OnWeaponsTabButtonClicked?.Invoke(this);
}
