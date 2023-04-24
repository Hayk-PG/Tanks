using System.Collections;
using UnityEngine;

public class BulletMotionSoundFX : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSrc;
    
    [SerializeField] [Space]
    private Rigidbody _rigidbody;

    //SecondarySoundController falling sound clips
    [Tooltip("SecondarySoundController falling sound clips")] [SerializeField] [Space]
    private int _clipIndex;

    private float _volume = 1;

    [SerializeField] [Space]
    private bool _notVelocityBased, _isDynamic, _isLoop;

    private bool _isCoroutineStarted;





    private void Awake()
    {
        _audioSrc.clip = SecondarySoundController.Clips[2]._clips[_clipIndex < 2 ? 2 : _clipIndex];
    }

    private void FixedUpdate()
    {
        if (!_isCoroutineStarted)
        {
            StartCoroutine(Play());

            _isCoroutineStarted = true;
        }
    }

    private IEnumerator Play()
    {
        yield return StartCoroutine(PlayIndependentAudioClip());
        yield return StartCoroutine(PlayVelocityBasedAudioClip());
    }

    // Suitable for short loop audio clips.
    private IEnumerator PlayIndependentAudioClip()
    {
        if (!_notVelocityBased)
            yield break;

        _audioSrc.Play();
        _audioSrc.loop = _isLoop;

        while (!SoundController.IsSoundMuted)
        {
            yield return null;
        }

        _audioSrc.mute = true;
    }

    // Suitable for long audio clips.
    private IEnumerator PlayVelocityBasedAudioClip()
    {
        if (_notVelocityBased)
            yield break;

        if (_rigidbody == null)
            yield break;

        _audioSrc.loop = _isLoop;

        bool isPlayed = false;

        while (_volume > 0.1f && !SoundController.IsSoundMuted)
        {
            if (_rigidbody.velocity.y < 0)
            {
                if (!isPlayed)
                {
                    _audioSrc.Play();

                    isPlayed = true;
                }

                if (_isDynamic)
                {
                    _volume -= 0.5f * Time.deltaTime;

                    _audioSrc.volume = _volume;
                }
            }

            yield return null;
        }

        _audioSrc.mute = true;
    }
}