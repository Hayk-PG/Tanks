using System;
using UnityEngine;


public class TankDamageFire : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _fireParticles;
    private HealthController _healthController;

    public Action<bool> OnTankDamageFire { get; set; }


    private void Awake() => _healthController = Get<HealthController>.From(gameObject);

    private void OnEnable()
    {
        if (_healthController != null)
            _healthController.OnTankDamageFire += OnHealthControllerTankDamageFire;
    }

    private void OnDisable()
    {
        if (_healthController != null)
            _healthController.OnTankDamageFire -= OnHealthControllerTankDamageFire;
    }

    private void OnParticle(ParticleSystem particle, bool play)
    {
        if (play && !particle.isPlaying)
            particle.Play(true);

        if (!play && particle.isPlaying)
            particle.Stop(true);
    }

    private void OnHealthControllerTankDamageFire(int health)
    {
        if(health <= 50 && health > 15)
        {
            OnParticle(_fireParticles[0], true);
            OnParticle(_fireParticles[1], false);
            OnTankDamageFire?.Invoke(false);
        }

        if(health <= 15)
        {
            OnParticle(_fireParticles[0], false);
            OnParticle(_fireParticles[1], true);
            OnTankDamageFire?.Invoke(false);
        }

        else if(health > 50)
        {
            OnParticle(_fireParticles[0], false);
            OnParticle(_fireParticles[1], false);
            OnTankDamageFire?.Invoke(true);
        }
    }    
}
