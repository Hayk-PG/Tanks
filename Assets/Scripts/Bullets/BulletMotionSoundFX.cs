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

    private bool _isSoundPlayed;



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
        if (!SoundController.IsMuted)
        {
            _audioSrc.enabled = true;
            _audioSrc.Play();

            while (_volume > 0.05f)
            {
                _volume -= 0.5f * Time.deltaTime;

                _audioSrc.volume = _volume;

                yield return null;
            }
        }
    }
}