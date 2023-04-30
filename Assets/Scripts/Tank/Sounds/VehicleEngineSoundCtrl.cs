using UnityEngine;

public class VehicleEngineSoundCtrl : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSRC;

    [SerializeField]
    private AudioClip _engineSound, _stopSound;

    private BaseTankMovement _baseTankMovement;

    private bool _hasStopSoundEnded, _isEngineSoundPlaying, _IsStopSoundPlayingOnBrake, _isEngineSoundPlayingOnMovement;


    private void Awake()
    {
        _baseTankMovement = Get<BaseTankMovement>.From(gameObject);
    }

    private void OnEnable()
    {
        if (_baseTankMovement != null) _baseTankMovement.OnVehicleMove += OnVehicleEngineSound;
    }

    private void OnDisable()
    {
        if (_baseTankMovement != null) _baseTankMovement.OnVehicleMove -= OnVehicleEngineSound;
    }

    private void OnVehicleEngineSound(float rpm)
    {
        _hasStopSoundEnded = audioSRC.clip == _stopSound &&
                                 audioSRC.time >= _stopSound.length - 0.05f &&
                                 audioSRC.volume > 0;
        _isEngineSoundPlaying = audioSRC.clip == _engineSound && audioSRC.volume < 1;
        _IsStopSoundPlayingOnBrake = rpm == 0 && audioSRC.clip != _stopSound;
        _isEngineSoundPlayingOnMovement = rpm != 0 && audioSRC.clip != _engineSound;


        Conditions<bool>.Compare(_hasStopSoundEnded, _isEngineSoundPlaying, OnSetVehicleSrcVolumeToZero, OnIncreaseVehicleSrcVolume, null, null);
        Conditions<bool>.Compare(rpm == 0, rpm != 0,
            () => audioSRC.pitch = 1,
            () => IncreaseVehSrcPitch(rpm)
            , null, null);
        Conditions<bool>.Compare(_IsStopSoundPlayingOnBrake, _isEngineSoundPlayingOnMovement,
            () => SwitchAudioClips(audioSRC, _stopSound, false),
            () => SwitchAudioClips(audioSRC, _engineSound, true), null, null);
    }

    private void OnSetVehicleSrcVolumeToZero()
    {
        audioSRC.volume = 0;
    }

    private void OnIncreaseVehicleSrcVolume()
    {
        audioSRC.volume = Mathf.Lerp(audioSRC.volume, 1, 10 * Time.deltaTime);
    }

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