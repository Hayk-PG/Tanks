using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public enum ButtonType { Shell, Rocket}
public enum DisplayType { MainWeaponsList, AvailableWeapon}

public class AmmoTypeButton : MonoBehaviour
{
    public AmmoStars _ammoStars;

    [Serializable]
    public struct Properties
    {
        public ButtonType _buttonType;

        public DisplayType _displayType;

        [SerializeField] [Space]
        private Button _button;

        [SerializeField] [Space]
        private CanvasGroup _canvasGroupLock, _canvasGroupPrice;

        [SerializeField] [Space]
        private Image _imgWeapon, _imgFrame;

        [SerializeField] [Space]
        private TMP_Text _txtName, _txtPrice, _txtQuantity;

        [SerializeField] [Space]
        public Color[] _colors; //0: Selected 1: Available 2: Locked
        
        public Button Button
        {
            get => _button;
        }

        public CanvasGroup CanvasGroupLock => _canvasGroupLock;
        public CanvasGroup CanvasGroupPrice => _canvasGroupPrice;

        public Sprite Image
        {
            get => _imgWeapon.sprite;
            set => _imgWeapon.sprite = value;
        }

        public string Name
        {
            get => _txtName.text;

            set => _txtName.text = value;
        }

        public Color ColorFrame
        {
            get => _imgFrame.color;
            set => _imgFrame.color = value;
        }

        public int Index { get; set; }
        public int Quantity
        {
            get
            {
                return int.Parse(_txtQuantity.text.Substring(1));
            }

            set
            {
                _txtQuantity.text = "x" + value;
            }
        }
        public int Price
        {
            get
            {
                return int.Parse(_txtPrice.text);
            }

            set
            {
                _txtPrice.text = value.ToString();
            }
        }
        public int PlayerScore { get; set; }

        public float BulletMaxForce { get; set; }
        public float BulletForceMaxSpeed { get; set; }

        public bool IsUnlocked
        {
            get
            {
                return Button.interactable && ColorFrame != _colors[2];
            }

            set
            {
                Button.interactable = value;

                GlobalFunctions.CanvasGroupActivity(_canvasGroupLock, !value);

                ColorFrame = value ? _colors[1] : _colors[2];
            }
        }
        public bool IsSelected
        {
            get
            {
                return ColorFrame == _colors[0] && IsUnlocked;
            }
            set
            {
                ColorFrame = value ? _colors[0] : _colors[1];
            }
        }  
    }

    public Properties _properties;

    private bool _isAutoSelected;

    public Action<AmmoTypeButton> OnClickAmmoTypeButton { get; set; }   


    public virtual void OnClickButton()
    {
        if (!_properties.IsSelected && _properties.IsUnlocked)
            OnClickAmmoTypeButton?.Invoke(this);
    }

    public void DisplayScoresToUnlock(int playerScore, int bulletsCount)
    {
        ControlPriceTagActivity();

        _properties.PlayerScore = playerScore;

        _properties.Quantity = bulletsCount;

        Conditions<bool>.Compare(_properties.PlayerScore >= _properties.Price, Unlock, Lock);

        AutoSelect();
    }

    private void ControlPriceTagActivity() => GlobalFunctions.CanvasGroupActivity(_properties.CanvasGroupPrice, _properties.Price <= 0 ? false : true);

    private void Unlock()
    {
        if (_properties.IsUnlocked)
            return;

        _properties.IsUnlocked = true;
    }

    private void Lock()
    {
        if (_properties.IsUnlocked)
            return;

        _properties.IsUnlocked = false;
    }

    private void AutoSelect()
    {
        if (_properties.Index == 0 && !_isAutoSelected)
        {
            _properties.IsUnlocked = true;

            _properties.IsSelected = true;

            _isAutoSelected = true;
        }
    }
}
