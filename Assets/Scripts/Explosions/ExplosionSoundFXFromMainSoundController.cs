public class ExplosionSoundFXFromMainSoundController : Button_Sound
{
    private void OnEnable()
    {
        ExplosionsSoundController.PlaySound(_listIndex, _clipIndex);
    }
}
