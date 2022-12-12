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
    }

    protected virtual void Set()
    {
        _isOn = !_isOn;
        SetSoundController();
        PlayerPrefsSetInt();
        ChangeIcon();
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
}
