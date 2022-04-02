public class PlayerDamageScreenTextSoundController : UIBaseSoundController
{
    private PlayerDamageScreenText _playerDamageScreenText;


    private void Awake()
    {
        _playerDamageScreenText = FindObjectOfType<PlayerDamageScreenText>();
    }

    private void OnEnable()
    {
        _playerDamageScreenText.OnSoundFX += OnSoundFX;
    }

    private void OnDisable()
    {
        _playerDamageScreenText.OnSoundFX -= OnSoundFX;
    }

    private void OnSoundFX()
    {
        PlaySoundFX(0);
    }
}
