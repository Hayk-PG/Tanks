using System;
using UnityEngine;
using UnityEngine.UI;

public class AmmoTypeButton : MonoBehaviour
{
    public AmmoStars _ammoStars;

    [Serializable]
    public struct Properties
    {
        [SerializeField]
        private Button _button;

        [SerializeField]
        private Image _buttonImage, _iconImage;

        [SerializeField]
        private Text _titleText;

        [SerializeField]
        internal Text _pointsToUnlockText;

        public Button Button { get => _button; }
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
        public string PointsToUnlock
        {
            get => _pointsToUnlockText.text;
            set => _pointsToUnlockText.text = value;
        }

        public int Index { get; set; }
        public int Value { get; set; }
        public int CurrentValue { get; set; }
        public int UnlockPoints { get; set; }
        public int CurrentPoints { get; set; }

        public bool IsUnlocked { get; set; }
        public bool IsSelected { get; set; }
    }
    public Properties _properties;

    public event Action<AmmoTypeButton> OnClickAmmoTypeButton;



    public virtual void OnClickButton()
    {
        if(!_properties.IsSelected) OnClickAmmoTypeButton?.Invoke(this);
    }

    public virtual void DisplayPointsToUnlock(int playerPoints, int bulletsCount)
    {
        _properties.CurrentPoints = playerPoints;
        _properties.CurrentValue = bulletsCount;
        _properties.PointsToUnlock = _properties.CurrentPoints + "/" + _properties.UnlockPoints;
        
        if (_properties.CurrentValue > 0)
        {
            _properties.IsUnlocked = true;
        }
        else
        {
            _properties.IsUnlocked = false;

            if (_properties.CurrentPoints < _properties.UnlockPoints)
            {
                _properties.Button.interactable = false;
            }
            else
            {
                _properties.Button.interactable = true;
            }            
        }

        _properties._pointsToUnlockText.gameObject.SetActive(!_properties.IsUnlocked);
    }    
}
