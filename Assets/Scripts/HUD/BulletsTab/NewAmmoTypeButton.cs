using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewAmmoTypeButton : MonoBehaviour
{
    //[SerializeField] private AmmoStars _ammoStarts;

    //[Serializable] public new struct Properties
    //{
    //    [SerializeField] private Button _button;
    //    [SerializeField] private CanvasGroup _canvasGroupWeaponsTab;
    //    [SerializeField] private CanvasGroup _canvasGroupSupportTab;
    //    [SerializeField] private CanvasGroup _canvasGroupTabScoresToUnlock;
    //    [SerializeField] private CanvasGroup _canvasGroupTimer;
    //    [SerializeField] private Image _imageWoodBoxIcon;
    //    [SerializeField] private Image _imageWalkieTalkieIcon;
    //    [SerializeField] private Image _imageWeaponsTabCheckBox;
    //    [SerializeField] private Image _imageSupportTabCheckBox;
    //    [SerializeField] private TMP_Text _textValue;
    //    [SerializeField] private TMP_Text _textRequiredScoreAmmount;
    //    [SerializeField] private TMP_Text _textSupportType;
    //    [SerializeField] private TMP_Text[] _textStatsNames;
    //    [SerializeField] private TMP_Text[] _textStatsValues;
        
    //    public Button Button
    //    {
    //        get => _button;
    //    }
    //    public CanvasGroup CanvasGroupTabScoresToUnlock
    //    {
    //        get => _canvasGroupTabScoresToUnlock;
    //    }
    //    public Sprite Icon
    //    {
    //        get
    //        {
    //            if (_canvasGroupWeaponsTab.interactable)
    //                return _imageWoodBoxIcon.sprite;
    //            else
    //                return _imageWalkieTalkieIcon.sprite;
    //        }
    //        set
    //        {
    //            if (_canvasGroupWeaponsTab.interactable)
    //                _imageWoodBoxIcon.sprite = value;
    //            else
    //                _imageWalkieTalkieIcon.sprite = value;
    //        }
    //    }
    //    public int Index { get; set; }
    //    public int Value
    //    {
    //        get => int.Parse(_textValue.text);
    //        set => _textValue.text = value.ToString();
    //    }
    //    public int CurrentValue { get; set; }
    //    public int RequiredScoreAmmount
    //    {
    //        get => int.Parse(_textRequiredScoreAmmount.text);
    //        set => _textRequiredScoreAmmount.text = value.ToString();
    //    }
    //    public int CurrentScoreAmmount { get; set; }
    //    public bool IsUnlocked { get; set; }
    //    public bool IsSelected
    //    {
    //        get
    //        {
    //            if (_canvasGroupWeaponsTab.interactable)
    //                return _imageWeaponsTabCheckBox.gameObject.activeInHierarchy;
    //            else
    //                return _imageSupportTabCheckBox.gameObject.activeInHierarchy;
    //        }
    //        set
    //        {
    //            if (_canvasGroupWeaponsTab.interactable)
    //                _imageWeaponsTabCheckBox.gameObject.SetActive(value);
    //            else
    //                _imageSupportTabCheckBox.gameObject.SetActive(value);
    //        }
    //    }
    //    public TMP_Text[] StatsNames
    //    {
    //        get => _textStatsNames;
    //    }
    //    public TMP_Text[] StatsValues
    //    {
    //        get => _textStatsValues;
    //    }      
    //}

    //public new Properties _properties;
    //public Action<NewAmmoTypeButton> OnClickNewAmmoTypeButton { get; set; }



    ////public override void OnClickButton()
    ////{
    ////    if (!_properties.IsSelected) OnClickNewAmmoTypeButton?.Invoke(this);
    ////}

    //public void DisplayScoresToUnlock(int playerScore, int bulletsCount)
    //{
    //    _properties.CurrentScoreAmmount = playerScore;
    //    _properties.CurrentValue = bulletsCount;

    //    if (_properties.CurrentValue > 0)
    //        _properties.IsUnlocked = true;
    //    else
    //    {
    //        _properties.IsUnlocked = false;

    //        if (_properties.CurrentScoreAmmount < _properties.RequiredScoreAmmount)
    //            _properties.Button.interactable = false;
    //        else
    //            _properties.Button.interactable = true;
    //    }

    //    GlobalFunctions.CanvasGroupActivity(_properties.CanvasGroupTabScoresToUnlock, !_properties.IsUnlocked);
    //}
}
