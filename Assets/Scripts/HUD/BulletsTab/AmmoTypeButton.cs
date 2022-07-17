﻿using System;
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
        [SerializeField] private Image _imageWoodBoxIcon;
        [SerializeField] private Image _imageWalkieTalkieIcon;
        [SerializeField] private Image _imageWeaponsTabCheckBox;
        [SerializeField] private Image _imageSupportTabCheckBox;
        [SerializeField] private TMP_Text _textValue;
        [SerializeField] private TMP_Text _textRequiredScoreAmmount;
        [SerializeField] private TMP_Text _textSupportType;
        [SerializeField] private TMP_Text[] _textStatsValues;
        [SerializeField] private TMP_Text[] _textStatsNames;       

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
                    _imageWeaponsTabCheckBox.gameObject.SetActive(value);
                else
                    _imageSupportTabCheckBox.gameObject.SetActive(value);
            }
        }       
    }

    public Properties _properties;

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
            _properties.IsUnlocked = true;
        else
        {
            _properties.IsUnlocked = false;

            if (_properties.CurrentScoreAmmount < _properties.RequiredScoreAmmount)
                _properties.Button.interactable = false;
            else
                _properties.Button.interactable = true;
        }

        GlobalFunctions.CanvasGroupActivity(_properties.CanvasGroupTabScoresToUnlock, !_properties.IsUnlocked);
    }
}
