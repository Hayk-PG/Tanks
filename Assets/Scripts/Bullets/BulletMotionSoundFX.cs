using System.Collections;
using UnityEngine;

public class BulletMotionSoundFX : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSrc;
    
    [SerializeField] [Space]
    private Rigidbody _rigidbody;

    [SerializeField] [Space]
    private float _volume;

    [SerializeField] [Space]
    private bool _noVolumeControlling;

    private bool _isSoundPlayed;




    private void Start() => PlayOnShot();

    private void FixedUpdate() => PlaySoundOnDescending();

    private void PlayOnShot()
    {
        if (_noVolumeControlling)
            StartCoroutine(PlayOnShotCoroutine());
    }

    private void PlaySoundOnDescending()
    {
        if (!_noVolumeControlling && _rigidbody != null && _rigidbody.velocity.y < 0 && !_isSoundPlayed)
        {
            StartCoroutine(PlaySoundOnDescendingCoroutine());

            _isSoundPlayed = true;
        }
    }

    private IEnumerator PlayOnShotCoroutine()
    {
        _audioSrc.enabled = true;
        _audioSrc.loop = true;
        _audioSrc.Play();

        while (!SoundController.IsSoundMuted)
        {
            _audioSrc.volume = 1;

            yield return null;
        }

        _audioSrc.mute = true;
    }

    private IEnumerator PlaySoundOnDescendingCoroutine()
    {
        _audioSrc.enabled = true;
        _audioSrc.Play();

        while (_volume > 0.05f && !SoundController.IsSoundMuted)
        {
            _volume -= 0.5f * Time.deltaTime;

            _audioSrc.volume = _volume;

            yield return null;
        }
    }
}