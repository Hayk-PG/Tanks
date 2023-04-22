using UnityEngine;
using System;

public class OptionsSound : OptionsController, ISoundController
{
    [SerializeField] 
    protected Sprite _sprtOn, _sprtOff;

    protected virtual bool IsMuted => SoundController.IsSoundMuted;

    protected virtual string Title =>  "Sound: ";

    protected virtual Action<bool?, ISoundController> ToggleAudioActivity => SoundController.ToggleSoundActivity;




    protected virtual void Start()
    {
        ChangeIcon();

        ChangeText();
    }

    protected override void Select() => ToggleAudioActivity?.Invoke(null, this);

    protected virtual void ChangeIcon() => _icon.sprite = IsMuted ? _sprtOff : _sprtOn;

    protected virtual void ChangeText()
    {
        string txt = IsMuted ? GlobalFunctions.TextWithColorCode("#FD1E40", "Off") : GlobalFunctions.TextWithColorCode("#1EFDB6", "On");

        _btnTxt.SetButtonTitle(Title + txt);
    }

    public virtual void Set(bool isOn = false)
    {
        ChangeIcon();

        ChangeText();
    }
}