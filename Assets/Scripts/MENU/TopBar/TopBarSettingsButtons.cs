using System;
using UnityEngine;
using UnityEngine.UI;

public class TopBarSettingsButtons : BaseButtonWithUnityEvent
{
    public enum ButtonPurpose { Sound, Music, BackMainMenu, Quit, Settings }
    public ButtonPurpose _buttonPurpose;

    public Action OnSettingsButtonClicked { get; set; }

    [SerializeField] private Image _iconImage;
    [SerializeField] private Sprite[] _iconSprites;
    private Sprite Icon
    {
        get => _iconImage.sprite;
        set => _iconImage.sprite = value;
    }
    private bool _isOn;


    private void Start()
    {  
        if (_buttonPurpose == ButtonPurpose.Music)
        {
            OnStart(Keys.IsMusicOn);
            SoundController.MusicSRCCondition(_isOn);
        }  
        
        if (_buttonPurpose == ButtonPurpose.Sound)
        {
            OnStart(Keys.IsSoundOn);
        }
    }

    private void GetCurrentCondition(string key)
    {
        int i = PlayerPrefs.GetInt(key, 1);
        _isOn = i == 0 ? false : true;
    }

    private int CurrentConditionIndex()
    {
        return !_isOn ? 0 : 1;
    }

    private void ChangeIcon()
    {
        if (_iconSprites.Length > 0)
        {
            Icon = _isOn ? _iconSprites[0] : _iconSprites[1];
        }
    }

    private void OnStart(string key)
    {
        GetCurrentCondition(key);       
        ChangeIcon();
    }

    public void OnClickSoundButton()
    {
        Result(Keys.IsSoundOn);
    }

    public void OnClickMusicButton()
    {
        Result(Keys.IsMusicOn);
        SoundController.MusicSRCCondition(_isOn);
    }

    private void Result(string key)
    {
        _isOn = !_isOn;
        ChangeIcon();
        PlayerPrefs.SetInt(key, CurrentConditionIndex());       
    }

    public void OnClickBackToMainMenuButton()
    {
        if (!MyPhotonNetwork.IsOfflineMode) MyPhoton.LeaveRoom();
        MyScene.Manager.LoadScene(MyScene.SceneName.Menu);
    }

    public void OnClickQuitButton()
    {
        Application.Quit();
    }

    public void OnClickSettingsButton()
    {
        OnSettingsButtonClicked?.Invoke();
    }    
}
