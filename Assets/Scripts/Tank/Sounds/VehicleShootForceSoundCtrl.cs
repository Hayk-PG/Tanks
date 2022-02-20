using UnityEngine;

public class VehicleShootForceSoundCtrl : VehicleCanonSoundCtrl
{
    private void OnEnable()
    {
        if(_shootController != null) _shootController.OnApplyingForce += OnApplyingForce;
    }

    private void OnDisable()
    {
        if (_shootController != null) _shootController.OnApplyingForce -= OnApplyingForce;
    }

    private void OnApplyingForce(bool isApplyingForce)
    {
        if (isApplyingForce && _audioSRC.volume != 1) _audioSRC.volume = 1;
        if (!isApplyingForce && _audioSRC.volume != 0) _audioSRC.volume = 0;
    }
}
