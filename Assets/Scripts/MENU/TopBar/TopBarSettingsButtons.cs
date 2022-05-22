using System;
using UnityEngine;
using UnityEngine.UI;

public class TopBarSettingsButtons : BaseButtonWithUnityEvent
{
    public enum ButtonPurpose { Sound, Music, BackMainMenu, Quit, Settings }
    public ButtonPurpose _buttonPurpose;

    public Action<bool> OnSoundButtonClicked { get; set; }
    public Action<bool> OnMusicButtonClicked { get; set; }
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
        _isOn = true;
        ChangeIcon(_isOn);
    }

    public void OnClickSoundButton()
    {
        _isOn = !_isOn;
        ChangeIcon(_isOn);
        OnSoundButtonClicked?.Invoke(_isOn);
    }

    public void OnClickMusicButton()
    {
        _isOn = !_isOn;
        ChangeIcon(_isOn);
        OnMusicButtonClicked?.Invoke(_isOn);
    }

    public void OnClickBackToMainMenuButton()
    {
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

    private void ChangeIcon(bool isOn)
    {
        if (_iconSprites.Length > 0)
        {
            Icon = isOn ? _iconSprites[0] : _iconSprites[1];
        }
    }
}
