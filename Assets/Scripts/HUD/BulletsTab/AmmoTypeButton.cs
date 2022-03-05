using System;
using UnityEngine;
using UnityEngine.UI;

public class AmmoTypeButton : MonoBehaviour
{
    public AmmoStars _ammoStars;

    [Serializable]
    public struct Properties
    {
        public Image _buttonImage, _iconImage;
        public Text _titleText;

        public Sprite ButtonSprite
        {
            get => _buttonImage.sprite;
            set => _buttonImage.sprite = value;
        }
        public Sprite IconSprite
        {
            get => _iconImage.sprite;
            set => _iconImage.sprite = value;
        }
        public string Title
        {
            get => _titleText.text;
            set => _titleText.text = value;
        }

        public int Index { get; set; }
    }
    public Properties _properties;

    public event Action<AmmoTypeButton> OnClickAmmoTypeButton;



    public virtual void OnClickButton()
    {
        OnClickAmmoTypeButton?.Invoke(this);
    }   
}
