using UnityEngine;

public class VehicleCanonSoundCtrl : MonoBehaviour
{
    [SerializeField] protected AudioSource _audioSRC;

    [SerializeField] protected AudioClip _clip;

    protected ShootController _shootController;


    protected virtual void Awake()
    {        
        _shootController = Get<ShootController>.From(gameObject);
    }

    protected virtual void Start()
    {
        SetAudioSrc();
    }

    private void OnEnable()
    {
        if(_shootController != null) 
            _shootController.OnCanonRotation += OnCanonRotation;
    }

    private void OnDisable()
    {
        if (_shootController != null) 
            _shootController.OnCanonRotation -= OnCanonRotation;
    }

    protected void SetAudioSrc()
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
        if(_audioSRC.volume != 1) 
            _audioSRC.volume = 1;
    }

    private void OnStopRotation()
    {
        if (_audioSRC.volume != 0)
            _audioSRC.volume = 0;
    }
}
