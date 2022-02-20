using UnityEngine;

public class VehicleSmoke : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private BaseTankMovement _tankMovement;


    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _tankMovement = Get<BaseTankMovement>.From(gameObject);
    }

    private void OnEnable()
    {
        if(_tankMovement != null) _tankMovement.OnVehicleMove += OnVehicleMove;
    }

    private void OnDisable()
    {
        if (_tankMovement != null) _tankMovement.OnVehicleMove -= OnVehicleMove;
    }

    private void OnVehicleMove(float rpm)
    {
        ParticleSystemActivtiy(rpm != 0);
    }

    private void ParticleSystemActivtiy(bool isEnabled)
    {
        if (!_particleSystem.isPlaying && isEnabled) _particleSystem.Play();
        if (_particleSystem.isPlaying && !isEnabled) _particleSystem.Stop();
    }
}
