using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


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
        private CanvasGroup _canvasGroupLock, _canvasGroupPrice, _canvasGroupTextOwned;

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
        public CanvasGroup CanvasGroupTextOwned => _canvasGroupTextOwned;

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
                return int.Parse(_txtQuantity.text);
            }

            set
            {
                _txtQuantity.text = Converter.DecimalString(value, 3);
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

    [SerializeField] [Space]
    private CanvasGroup _canvasGroupDisplay, _canvasGroupDescription;

    [SerializeField] [Space]
    private TMP_Text _txtTop, _txtBottom;

    private bool _isAutoSelected;

    public Action<AmmoTypeButton> OnClickAmmoTypeButton { get; set; }




    private void Awake() => SetGroupsActive(GameSceneObjectsReferences.AmmoTabDescriptionButton.IsActive);

    private void OnEnable() => GameSceneObjectsReferences.AmmoTabDescriptionButton.onDescriptionActivity += SetGroupsActive;

    public virtual void OnClickButton()
    {
        if (!_properties.IsSelected && _properties.IsUnlocked)
            OnClickAmmoTypeButton?.Invoke(this);
    }

    private void SetGroupsActive(bool isDescription)
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroupDisplay, !isDescription);
        GlobalFunctions.CanvasGroupActivity(_canvasGroupDescription, isDescription);
    }

    public void DisplayScoresToUnlock(int playerScore, int bulletsCount)
    {
        SetPriceTagActive();

        SetTextOwnedActive();

        _properties.PlayerScore = playerScore;

        _properties.Quantity = bulletsCount;

        Conditions<bool>.Compare(_properties.PlayerScore >= _properties.Price, Unlock, Lock);

        AutoSelect();
    }

    public void PrintDescription(string title, string text)
    {
        if (_txtTop == null || _txtBottom == null)
            return;

        _txtTop.text = title;
        _txtBottom.text = text;
    }

    private void SetPriceTagActive() => GlobalFunctions.CanvasGroupActivity(_properties.CanvasGroupPrice, _properties.Price <= 0 ? false : true);

    private void SetTextOwnedActive() => GlobalFunctions.CanvasGroupActivity(_properties.CanvasGroupTextOwned, _properties.Price <= 0 ? true : false);

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
