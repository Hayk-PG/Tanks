using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class ExplosionSoundFX : MonoBehaviour
{
    private AudioSource _audioSrc;

    [SerializeField]
    private AudioClip[] _clips;


    private void Awake()
    {
        _audioSrc = Get<AudioSource>.From(gameObject);
        _audioSrc.clip = _clips[Random.Range(0, _clips.Length)];
        _audioSrc.Play();
    }
}
