using System.Collections;
using UnityEngine;

public class BulletMotionSoundFX : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSrc;
    [SerializeField] private float _volume;
    private float _defaultVolume;
    private bool _isSoundPlayed;
    private Rigidbody _rigidbody;
 


    private void Awake()
    {
        _rigidbody = GetComponentInParent<Rigidbody>();
        _defaultVolume = _volume;
    }

    private void FixedUpdate()
    {
        if (_rigidbody != null && _rigidbody.velocity.y < 0 && !_isSoundPlayed)
        {
            StartCoroutine(SoundCoroutine());
        }

        if(_rigidbody != null && _rigidbody.velocity.y > 0 && _isSoundPlayed)
        {
            StopSound();
        }
    }

    private IEnumerator SoundCoroutine()
    {
        _isSoundPlayed = true;
        _volume = _defaultVolume;
        _audioSrc.enabled = _isSoundPlayed;
        _audioSrc.Play();
        
        
        while (_rigidbody.velocity.y < 0)
        {            
            _volume -= 0.5f * Time.deltaTime;
            _audioSrc.volume = _volume;
            yield return null;
        }
    }

    private void StopSound()
    {
        _audioSrc.volume = _defaultVolume;
        _audioSrc.Stop();
        _isSoundPlayed = false;
    }
}

