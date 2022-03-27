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
        if (_tempPoints != null) _tempPoints.OnTempPointsMotionSoundFX += OnTempPointsMotionSoundFX;
        if (_tempPoints != null) _tempPoints.OnScoreTextUpdated += OnTempPointsReachedSoundFX;
    }

    private void OnDisable()
    {
        if (_tempPoints != null) _tempPoints.OnTempPointsMotionSoundFX -= OnTempPointsMotionSoundFX;
        if (_tempPoints != null) _tempPoints.OnScoreTextUpdated += OnTempPointsReachedSoundFX;
    }

    private void OnTempPointsMotionSoundFX()
    {
        PlaySoundFX(_audioSrc, _clips[0]);
    }

    private void OnTempPointsReachedSoundFX(int score)
    {
        PlaySoundFX(_audioSrc, _clips[1]);
    }

    private void PlaySoundFX(AudioSource audioSrc, AudioClip clip)
    {
        audioSrc.PlayOneShot(clip);
    }
}
