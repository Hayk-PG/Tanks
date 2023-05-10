using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public enum ButtonType { Shell, Rocket, Railgun }
public enum DisplayType { MainWeaponsList, AvailableWeapon}

public class AmmoTypeButton : MonoBehaviour, IRequiredPointsManager
{
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

        private bool _isUnlocked;
        
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

        public float BulletMaxForce { get; set; }
        public float BulletForceMaxSpeed { get; set; }

        public bool IsUnlocked
        {
            get => _isUnlocked && ColorFrame != _colors[2];

            set
            {
                _isUnlocked = value;

                GlobalFunctions.CanvasGroupActivity(_canvasGroupLock, !value);

                if (!value)
                {
                    ColorFrame = _colors[2];

                    return;
                }

                ColorFrame = IsSelected ? _colors[0] : _colors[1];
            }
        }

        public bool IsSelected
        {
            get => ColorFrame == _colors[0] && IsUnlocked;

            set
            {
                if (!IsUnlocked)
                {
                    ColorFrame = _colors[2];

                    return;
                }

                ColorFrame = value ? _colors[0] : _colors[1];
            }
        }  
    }

    public AmmoStars _ammoStars;

    public Properties _properties;

    [SerializeField] [Space]
    private CanvasGroup _canvasGroupDisplay, _canvasGroupDescription;

    [SerializeField] [Space]
    private TMP_Text _txtTop, _txtBottom;

    private int _defaultQuantity;

    private bool _isAutoSelected;

    private ScoreController LocalPlayerScoreController { get; set; }

    public Action<AmmoTypeButton> OnClickAmmoTypeButton { get; set; }

    





    private void Awake()
    {
        SetGroupsActive(GameSceneObjectsReferences.AmmoTabDescriptionButton.IsActive);
    }

    private void Start() => SetDefaultQuantity();

    private void OnEnable() => GameSceneObjectsReferences.AmmoTabDescriptionButton.onDescriptionActivity += SetGroupsActive;

    private void SetDefaultQuantity()
    {
        _defaultQuantity = _properties.Quantity;

        print(_defaultQuantity);
    }

    private void SetGroupsActive(bool isDescription)
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroupDisplay, !isDescription);
        GlobalFunctions.CanvasGroupActivity(_canvasGroupDescription, isDescription);
    }

    public void GetLocalPlayerScoreController(ScoreController scoreController) => LocalPlayerScoreController = scoreController;

    public void PrintDescription(string title, string text)
    {
        bool canPrintDescription = _txtTop != null && _txtBottom != null;

        if (!canPrintDescription)
            return;

        _txtTop.text = title;

        _txtBottom.text = text;
    }

    public virtual void OnClickButton()
    {
        if (IsSelected())
            return;

        if (IsUnlocked())
        {
            OnClickAmmoTypeButton?.Invoke(this);

            return;
        }

        UnlockAndDeductPointsFromPlayer();
    }

    private void UnlockAndDeductPointsFromPlayer()
    {
        if (!HasEnoughPoints())
            return;

        Unlock();

        IncrementRequiredPoints(1000);

        LocalPlayerScoreController.GetScore(-_properties.Price, null);

        OnClickAmmoTypeButton?.Invoke(this);
    }

    public void HandleAmmoButtonAvailability(int ammoCount)
    {
        GetLocaPlayerAmmoCount(ammoCount);

        if (!IsPointsRequired())
        {
            HandleUnlockState(true);

            ToggleCostUIVisibility(false);
        }

        if(IsPointsRequired() && !IsAmmoAvailable(ammoCount))
        {
            HandleUnlockState(false);

            ToggleCostUIVisibility(true);

            ResetQuantity();
        }

        if(IsPointsRequired() && IsAmmoAvailable(ammoCount) && IsUnlocked())
        {
            HandleUnlockState(true);

            ToggleCostUIVisibility(false);
        }

        AutoSelect();
    }

    private void GetLocaPlayerAmmoCount(int ammoCount) => _properties.Quantity = ammoCount;

    private void ResetQuantity() => _properties.Quantity = _defaultQuantity;

    private bool IsSelected()
    {
        return _properties.IsSelected;
    }

    private bool IsUnlocked()
    {
        return _properties.IsUnlocked;
    }

    private bool HasEnoughPoints()
    {
        return LocalPlayerScoreController.Score >= _properties.Price;
    }

    private bool IsPointsRequired()
    {
        return _properties.Price > 0;
    }

    private bool IsAmmoAvailable(int ammoCount)
    {
        return ammoCount > 0;
    }

    private void ToggleCostUIVisibility(bool isCostUIActive)
    {
        GlobalFunctions.CanvasGroupActivity(_properties.CanvasGroupPrice, isCostUIActive);
        GlobalFunctions.CanvasGroupActivity(_properties.CanvasGroupTextOwned, !isCostUIActive);
    }

    private void HandleUnlockState(bool unlcok) => Conditions<bool>.Compare(unlcok, Unlock, Lock);

    private void Unlock()
    {
        if (_properties.IsUnlocked)
            return;

        _properties.IsUnlocked = true;
    }

    private void Lock()
    {
        if (!_properties.IsUnlocked)
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

    public void IncrementRequiredPoints(int amount = 0) => _properties.Price += amount;
}
