using System.Collections;
using UnityEngine;

public class BulletMotionSoundFX : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSrc;
    [SerializeField] private float _volume;     
    private bool _isSoundPlayed;
    private Rigidbody _rigidbody;



    private void Awake()
    {
        _rigidbody = GetComponentInParent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (_rigidbody != null && _rigidbody.velocity.y < 0 && !_isSoundPlayed)
        {
            StartCoroutine(SoundCoroutine());
            _isSoundPlayed = true;
        }
    }

    private IEnumerator SoundCoroutine()
    {
        _audioSrc.enabled = true;
        _audioSrc.Play();

        while (true)
        {            
            _volume -= 0.5f * Time.deltaTime;
            _audioSrc.volume = _volume;
            yield return null;
        }
    }
}

