using UnityEngine;

public class OptionsSound : OptionsController
{
    [SerializeField] protected Sprite _sprtOn, _sprtOff;

    protected bool _isOn;

    protected virtual string Key { get; } = Keys.IsSoundOn;


    protected virtual void Start()
    {
        SetDefault();
    }

    protected override void Select() => Set();

    public override void SetDefault() => Get();

    protected virtual void Get()
    {
        _isOn = PlayerPrefsGetInt() < 1 ? false : true;
        SetSoundController();
        ChangeIcon();
        ChangeText("Sound: ");
    }

    protected virtual void Set()
    {
        _isOn = !_isOn;
        SetSoundController();
        PlayerPrefsSetInt();
        ChangeIcon();
        ChangeText("Sound: ");
    }

    protected virtual int PlayerPrefsGetInt()
    {
        return PlayerPrefs.GetInt(Key, 0);
    }

    protected virtual void PlayerPrefsSetInt()
    {
        int index = _isOn ? 1 : 0;
        PlayerPrefs.SetInt(Key, index);
    }

    protected virtual void SetSoundController() => SoundController.SoundSRCCondition(_isOn);

    protected virtual void ChangeIcon() => _icon.sprite = _isOn ? _sprtOn : _sprtOff;

    protected virtual void ChangeText(string title)
    {
        string txt = _isOn ? GlobalFunctions.TextWithColorCode("#1EFDB6", "On") : GlobalFunctions.TextWithColorCode("#FD1E40", "Off");

        _btnTxt.SetButtonTitle(title + txt);
    }
}
