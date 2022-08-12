using System.Collections;
using UnityEngine;

public class BulletMotionSoundFX : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSrc;
    private float _volume = 1;


    public void PlaySound()
    {
        StartCoroutine(SoundCoroutine());
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

