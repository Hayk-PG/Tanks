using System;

public class OptionsMusic : OptionsSound
{
    protected override bool IsMuted => SoundController.IsMusicMuted;

    protected override string Title => "Music: ";

    protected override Action<bool?, ISoundController> ToggleAudioActivity => SoundController.ToggleMusicActivity;
}
