public class OptionsMusic : OptionsSound
{
    protected override string Key => Keys.IsMusicOn;

    protected override void SetSoundController() => SoundController.MusicSRCCondition(_isOn);

    protected override void ChangeText(string title)
    {
        base.ChangeText("Music: ");
    }
}
