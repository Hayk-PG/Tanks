using UnityEngine;

public class VehicleEngineSoundCtrl : MonoBehaviour
{
    [SerializeField] private AudioSource audioSRC;  
    [SerializeField] private AudioClip _engineSound, _stopSound;

    private BaseTankMovement _baseTankMovement;


    private void Awake() => _baseTankMovement = Get<BaseTankMovement>.From(gameObject);

    private void Start() => audioSRC.mute = SoundController.IsMuted;

    private void OnEnable()
    {
        if (!audioSRC.mute)
            AddListener();

        SoundController.onAudioSourceMute += OnAudioSourceMute;
    }

    private void OnDisable()
    {
        RemoveListener();
        SoundController.onAudioSourceMute -= OnAudioSourceMute;
    }

    private void AddListener()
    {
        if (_baseTankMovement != null)
            _baseTankMovement.OnVehicleMove += OnVehicleEngineSound;
    }

    private void RemoveListener()
    {
        if (_baseTankMovement != null)
            _baseTankMovement.OnVehicleMove -= OnVehicleEngineSound;
    }

    private void OnAudioSourceMute(bool isMuted)
    {
        audioSRC.mute = isMuted;
        RemoveListener();

        if (!isMuted)
            AddListener();
    }

    private void OnVehicleEngineSound(float rpm)
    {
        Conditions<bool>.Compare(IsSoundEnded(), IsEngineSoundPlaying(), OnSetVehicleSrcVolumeToZero, OnIncreaseVehicleSrcVolume, null, null);
        Conditions<bool>.Compare(rpm == 0, rpm != 0,
            () => audioSRC.pitch = 1,
            () => IncreaseVehSrcPitch(rpm)
            , null, null);
        Conditions<bool>.Compare(IsStopSoundPlayingOnBrake(rpm), IsEngineSoundPlayingOnMovement(rpm),
            () => SwitchAudioClips(audioSRC, _stopSound, false),
            () => SwitchAudioClips(audioSRC, _engineSound, true), null, null);
    }

    private bool IsSoundEnded()
    {
        return audioSRC.clip == _stopSound && audioSRC.time >= _stopSound.length - 0.05f && audioSRC.volume > 0;
    }

    private bool IsEngineSoundPlaying()
    {
        return audioSRC.clip == _engineSound && audioSRC.volume < 1;
    }

    private bool IsStopSoundPlayingOnBrake(float rpm)
    {
        return rpm == 0 && audioSRC.clip != _stopSound;
    }

    private bool IsEngineSoundPlayingOnMovement(float rpm)
    {
        return rpm != 0 && audioSRC.clip != _engineSound;
    }

    private void OnSetVehicleSrcVolumeToZero() => audioSRC.volume = 0;

    private void OnIncreaseVehicleSrcVolume() => audioSRC.volume = Mathf.Lerp(audioSRC.volume, 1, 10 * Time.deltaTime);

    private void IncreaseVehSrcPitch(float rpm)
    {
        Conditions<bool>.Compare(audioSRC.pitch, 2,
            delegate { audioSRC.pitch = 2; },
            delegate { audioSRC.pitch = 2; },
            delegate { audioSRC.pitch = Mathf.Lerp(audioSRC.pitch, 1 + Mathf.Abs(rpm) / 1000, 5 * Time.deltaTime); });
    }

    private void SwitchAudioClips(AudioSource src, AudioClip clip, bool loop)
    {
        src.Stop();
        src.clip = clip;
        src.loop = loop;
        src.Play();        
    }
}
