using UnityEngine;

public class VehicleCanonSoundCtrl : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSRC;

    [SerializeField]
    private AudioClip _clip;

    private ICanonRotation _iCanonRotation;


    private void Awake()
    {        
        _iCanonRotation = Get<ICanonRotation>.From(gameObject);
    }

    private void Start()
    {
        SetAudioSrc();
    }

    private void OnEnable()
    {
        if(_iCanonRotation != null) _iCanonRotation.OnCanonRotation += OnCanonRotation;
    }

    private void OnDisable()
    {
        if (_iCanonRotation != null) _iCanonRotation.OnCanonRotation -= OnCanonRotation;
    }

    private void SetAudioSrc()
    {
        _audioSRC.volume = 0;
        _audioSRC.Stop();
        _audioSRC.clip = _clip;
        _audioSRC.Play();
    }

    private void OnCanonRotation(bool isRotating)
    {
        Conditions<bool>.Compare(isRotating, !isRotating, OnRotation, OnStopRotation, null, null);
    }

    private void OnRotation()
    {
        if(_audioSRC.volume != 1) _audioSRC.volume = 1;
    }

    private void OnStopRotation()
    {
        if (_audioSRC.volume != 0) _audioSRC.volume = 0;
    }
}
