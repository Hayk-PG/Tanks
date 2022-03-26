using UnityEngine;

public class HUDSoundController : MonoBehaviour
{
    private AudioSource _audioSrc;
    private TempPoints _tempPoints;

    /// <summary>
    /// 0:(UI Movement) WHOOSH_AIRY_FLUTTER_01
    /// 1:(UI Score) Powerup upgrade 35
    /// </summary>
    [SerializeField]
    private AudioClip[] _clips;

    private delegate bool Checker();
    private Checker _isLocalPlayer;

    private void Awake()
    {
        _audioSrc = Get<AudioSource>.From(gameObject);
        _tempPoints = FindObjectOfType<TempPoints>();
    }

    private void OnEnable()
    {
        if (_tempPoints != null) _tempPoints.OnPlayerTempPoints += OnLocalPlayerTempPointsSoundFX;
        if (_tempPoints != null) _tempPoints.OnUpdateScore += OnUpdateScore;
    }

    private void OnDisable()
    {
        if (_tempPoints != null) _tempPoints.OnPlayerTempPoints -= OnLocalPlayerTempPointsSoundFX;
        if (_tempPoints != null) _tempPoints.OnUpdateScore += OnUpdateScore;
    }

    private void OnLocalPlayerTempPointsSoundFX()
    {
        PlaySoundFX(_audioSrc, _clips[0]);
    }

    private void OnUpdateScore(int score)
    {
        PlaySoundFX(_audioSrc, _clips[1]);
    }

    private void PlaySoundFX(AudioSource audioSrc, AudioClip clip)
    {
        audioSrc.PlayOneShot(clip);
    }
}
