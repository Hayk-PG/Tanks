using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Coffee.UIEffects;


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
        private UIShiny _uiShinyLock;

        [SerializeField] [Space]
        private Image _imgWeapon, _imgFrame;

        [SerializeField] [Space]
        private TMP_Text _txtName, _txtPrice, _txtQuantity;

        [SerializeField] [Space]
        private Color[] _colors; //0: Selected 1: Available 2: Locked

        private bool _isUnlocked;
        
        public Button Button
        {
            get => _button;
        }

        public CanvasGroup CanvasGroupLock => _canvasGroupLock;
        public CanvasGroup CanvasGroupPrice => _canvasGroupPrice;
        public CanvasGroup CanvasGroupTextOwned => _canvasGroupTextOwned;

        public UIShiny UiShinyLock => _uiShinyLock;

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
        public int DefaultQuantity { get; set; }
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
        public int RequiredPointsIncrementAmount { get; set; }

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

                GlobalFunctions.CanvasGroupActivity(_canvasGroupLock, !value);

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
        public bool IsAutoSelected { get; set; }
        public bool IsReusable { get; set; }
    }

    public AmmoStars _ammoStars;

    public Properties _properties;

    [SerializeField] [Space]
    private CanvasGroup _canvasGroupDisplay, _canvasGroupDescription;

    [SerializeField] [Space]
    private TMP_Text _txtTop, _txtBottom;

    private ScoreController LocalPlayerScoreController { get; set; }
    private PlayerAmmoType LocalPlayerAmmoType { get; set; }

    public Action<AmmoTypeButton> OnClickAmmoTypeButton { get; set; }

    





    private void Awake()
    {
        SetGroupsActive(GameSceneObjectsReferences.AmmoTabDescriptionButton.IsActive);
    }

    private void Start() => SetDefaultQuantity();

    private void OnEnable() => GameSceneObjectsReferences.AmmoTabDescriptionButton.onDescriptionActivity += SetGroupsActive;

    private void SetDefaultQuantity() => _properties.DefaultQuantity = _properties.Quantity;

    private void SetGroupsActive(bool isDescription)
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroupDisplay, !isDescription);
        GlobalFunctions.CanvasGroupActivity(_canvasGroupDescription, isDescription);
    }

    public void InitPlayerScoreAndAmmoType(ScoreController localPlayerScoreController, PlayerAmmoType localPlayerAmmoType)
    {
        LocalPlayerScoreController = localPlayerScoreController;

        LocalPlayerAmmoType = localPlayerAmmoType;
    }

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
        if(HasEnoughPoints() && IsReusable())
        {
            Unlock();

            UpdatePlayerAmmoOnUnlock();

            LocalPlayerScoreController.GetScore(-_properties.Price, null, null, transform.position, true, false);

            OnClickAmmoTypeButton?.Invoke(this);

            IncrementRequiredPoints(_properties.RequiredPointsIncrementAmount);
        }
    }

    public void HandleAmmoButtonAvailability(int ammoCount)
    {
        DeactivateIfNotReusableAndOutOfAmmo(ammoCount);

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

        PlayUiShinyEffect();

        AutoSelect();
    }

    private void DeactivateIfNotReusableAndOutOfAmmo(int ammoCount)
    {
        if (!IsReusable() && ammoCount <= 0)
            gameObject.SetActive(false);
    }

    private void GetLocaPlayerAmmoCount(int ammoCount) => _properties.Quantity = ammoCount;

    private void ResetQuantity() => _properties.Quantity = _properties.DefaultQuantity;

    private void UpdatePlayerAmmoOnUnlock()
    {
        bool hasAmmoRemaining = LocalPlayerAmmoType._weaponsBulletsCount[_properties.Index] > 0;

        if (hasAmmoRemaining)
            return;

        LocalPlayerAmmoType._weaponsBulletsCount[_properties.Index] = _properties.DefaultQuantity;
    }

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

    private bool IsReusable()
    {
        return _properties.IsReusable;
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

    private void PlayUiShinyEffect() => _properties.UiShinyLock.Play();

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
        if (_properties.Index == 0 && !_properties.IsAutoSelected)
        {
            _properties.IsUnlocked = true;

            _properties.IsSelected = true;

            _properties.IsAutoSelected = true;
        }
    }

    public void IncrementRequiredPoints(int amount = 0) => _properties.Price += amount;
}
