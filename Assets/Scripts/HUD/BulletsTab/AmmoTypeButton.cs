using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public enum ButtonType { Weapon, Support, Props }

public class AmmoTypeButton : MonoBehaviour
{
    public AmmoStars _ammoStars;

    [Serializable]
    public struct Properties
    {
        public ButtonType _buttonType;
        [SerializeField] private Button _button;
        [SerializeField] private CanvasGroup _canvasGroupWeaponsTab;
        [SerializeField] private CanvasGroup _canvasGroupSupportTab;
        [SerializeField] private CanvasGroup _canvasGroupTabScoresToUnlock;
        [SerializeField] private CanvasGroup _canvasGroupTimer;
        [SerializeField] private CanvasGroup _canvasGroupAmmount;
        [SerializeField] private Image _imageWoodBoxIcon;
        [SerializeField] private Image _imageWalkieTalkieIcon;
        [SerializeField] private Image _imageWeaponsTabCheckBox;
        [SerializeField] private Image _imageSupportTabCheckBox;
        [SerializeField] private Image _imageButton;
        [SerializeField] private TMP_Text _textTimer;
        [SerializeField] private TMP_Text _textValue;
        [SerializeField] private TMP_Text _textRequiredScoreAmmount;
        [SerializeField] private TMP_Text _textSupportType;
        [SerializeField] private TMP_Text[] _textStatsValues;
        [SerializeField] private TMP_Text[] _textStatsNames;
        [SerializeField] private Sprite[] _sprites;

        public Button Button
        {
            get => _button;
        }
        public CanvasGroup CanvasGroupWeaponsTab
        {
            get => _canvasGroupWeaponsTab;
        }
        public CanvasGroup CanvasGroupSupportTab
        {
            get => _canvasGroupSupportTab;
        }
        public CanvasGroup CanvasGroupTabScoresToUnlock
        {
            get => _canvasGroupTabScoresToUnlock;
        }
        public CanvasGroup CanvasGroupTimer
        {
            get => _canvasGroupTimer;
        }
        public CanvasGroup CanvasGroupAmmount
        {
            get => _canvasGroupAmmount;
        }
        public Sprite Icon
        {
            get
            {
                if (_canvasGroupWeaponsTab.interactable)
                    return _imageWoodBoxIcon.sprite;
                else
                    return _imageWalkieTalkieIcon.sprite;
            }
            set
            {
                if (_canvasGroupWeaponsTab.interactable)
                    _imageWoodBoxIcon.sprite = value;
                else
                    _imageWalkieTalkieIcon.sprite = value;
            }
        }
        public int Index { get; set; }
        public int Value
        {
            get => int.Parse(_textValue.text);
            set => _textValue.text = value.ToString();
        }
        public int CurrentValue { get; set; }
        public int RequiredScoreAmmount
        {
            get => int.Parse(_textRequiredScoreAmmount.text);
            set => _textRequiredScoreAmmount.text = value.ToString();
        }
        public int CurrentScoreAmmount { get; set; }
        public int DamageValue
        {
            get => int.Parse(_textStatsValues[0].text);
            set => _textStatsValues[0].text = value.ToString();
        }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
        public float BulletMaxForce { get; set; }
        public float BulletForceMaxSpeed { get; set; }
        public string MassValue
        {
            get => _textStatsValues[1].text;
            set => _textStatsValues[1].text = value;
        }
        public string WeaponType
        {
            get => _textStatsValues[2].text;
            set => _textStatsValues[2].text = value;
        }
        public string SupportOrPropsType
        {
            get => _textSupportType.text;
            set => _textSupportType.text = value;
        }
        public string Timer
        {
            get => _textTimer.text;
            set => _textTimer.text = value;
        }
        public bool IsUnlocked { get; set; }
        public bool IsSelected
        {
            get
            {
                if (_canvasGroupWeaponsTab.interactable)
                    return _imageWeaponsTabCheckBox.gameObject.activeInHierarchy;
                else
                    return _imageSupportTabCheckBox.gameObject.activeInHierarchy;
            }
            set
            {
                if (_canvasGroupWeaponsTab.interactable)
                {
                    _imageWeaponsTabCheckBox.gameObject.SetActive(value);
                    MainSprite = MainSpriteVariations[0];
                }
                else
                {
                    _imageSupportTabCheckBox.gameObject.SetActive(value);
                    MainSprite = MainSpriteVariations[0];
                }
            }
        }  
        public Sprite MainSprite
        {
            get => _imageButton.sprite;
            set => _imageButton.sprite = value;
        }
        public Sprite[] MainSpriteVariations
        {
            get => _sprites;
        }
    }

    public Properties _properties;

    private int _minutes;
    private int _seconds;
    private bool _isTimerFinished;

    public Action<AmmoTypeButton> OnClickAmmoTypeButton { get; set; }   
    public Action<AmmoTypeButton> OnClickSupportTypeButton { get; set; }
    public Action<AmmoTypeButton> OnClickPropsTypeButton { get; set; }



    public virtual void OnClickButton()
    {
        if (!_properties.IsSelected)
        {
            if (_properties._buttonType == ButtonType.Weapon) OnClickAmmoTypeButton?.Invoke(this);
            if (_properties._buttonType == ButtonType.Support) OnClickSupportTypeButton?.Invoke(this);
            if (_properties._buttonType == ButtonType.Props) OnClickPropsTypeButton?.Invoke(this);
        }
    }

    public void DisplayScoresToUnlock(int playerScore, int bulletsCount)
    {
        _properties.CurrentScoreAmmount = playerScore;
        _properties.CurrentValue = bulletsCount;

        if (_properties.CurrentValue > 0)
        {
            _properties.IsUnlocked = true;
            _properties.MainSprite = _properties.MainSpriteVariations[_properties.IsSelected ? 0: 1];
        }
        else
        {
            _properties.IsUnlocked = false;
            _properties.MainSprite = _properties.MainSpriteVariations[2];

            if (_properties.CurrentScoreAmmount < _properties.RequiredScoreAmmount || _properties.CanvasGroupTimer.interactable)
            {
                _properties.Button.interactable = false;
            }
            else if (_properties.CurrentScoreAmmount >= _properties.RequiredScoreAmmount && !_properties.CanvasGroupTimer.interactable)
            {
                _properties.Button.interactable = true;
                _properties.MainSprite = _properties.MainSpriteVariations[1];
            }
        }

        GlobalFunctions.CanvasGroupActivity(_properties.CanvasGroupTabScoresToUnlock, !_properties.IsUnlocked);
    }

    public void StartTimerCoroutine()
    {
        StartCoroutine(TimerCoroutine());
    }

    private IEnumerator TimerCoroutine()
    {
        OnTimerActivity(true);
        _minutes = _properties.Minutes;
        _seconds = _properties.Seconds;
        _isTimerFinished = false;

        while (!_isTimerFinished)
        {
            _seconds--;

            if (_seconds <= 0)
                Conditions<int>.Compare(_minutes, 0, OnReset, OnDecrease, OnReset);

            _properties.Timer = _minutes.ToString("D2") + ":" + _seconds.ToString("D2");
            yield return new WaitForSeconds(1);
        }
    }

    private void OnTimerActivity(bool isActive)
    {
        GlobalFunctions.CanvasGroupActivity(_properties.CanvasGroupTimer, isActive);
        GlobalFunctions.CanvasGroupActivity(_properties.CanvasGroupAmmount, !isActive);
    }

    private void OnDecrease()
    {
        _seconds = 60;
        _minutes--;
    }

    private void OnReset()
    {
        _isTimerFinished = true;
        OnTimerActivity(false);
    }
}
