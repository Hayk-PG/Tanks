using UnityEngine;

public class SoundFxVariation : MonoBehaviour
{
    private AudioSource _audioSRC;
    [SerializeField] private float _minPitch, _maxPitch;
    [SerializeField] private float _minVolume, _maxVolume;



    private void Awake()
    {
        _audioSRC = Get<AudioSource>.From(gameObject);
        Play();
    }

    private void Play()
    {
        _audioSRC.pitch = Random.Range(_minPitch, _maxPitch);
        _audioSRC.volume = Random.Range(_minVolume, _maxVolume);
        _audioSRC.Play();
    }
}
