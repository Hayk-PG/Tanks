using UnityEngine;

public class VehicleSmoke : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private BaseTankMovement _tankMovement;
    private TankDamageFire _tankDamageFire;
    private bool _canPlaySmokeParticles = true;


    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _tankMovement = Get<BaseTankMovement>.From(gameObject);
        _tankDamageFire = Get<TankDamageFire>.FromChild(_tankMovement.gameObject);
    }

    private void OnEnable()
    {
        if(_tankMovement != null) _tankMovement.OnVehicleMove += OnVehicleMove;
        if (_tankDamageFire != null) _tankDamageFire.OnTankDamageFire += OnTankDamageFire;
    }

    private void OnDisable()
    {
        if (_tankMovement != null) _tankMovement.OnVehicleMove -= OnVehicleMove;
        if (_tankDamageFire != null) _tankDamageFire.OnTankDamageFire -= OnTankDamageFire;
    }

    private void OnVehicleMove(float rpm)
    {
        ParticleSystemActivtiy(rpm != 0 && _canPlaySmokeParticles);
    }

    private void ParticleSystemActivtiy(bool isEnabled)
    {
        if (!_particleSystem.isPlaying && isEnabled) _particleSystem.Play();
        if (_particleSystem.isPlaying && !isEnabled) _particleSystem.Stop();
    }

    private void OnTankDamageFire(bool canPlaySmokeParticles)
    {
        _canPlaySmokeParticles = canPlaySmokeParticles;
    }
}
