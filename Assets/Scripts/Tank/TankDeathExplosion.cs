using CartoonFX;
using System.Collections;
using UnityEngine;

public class TankDeathExplosion : MonoBehaviour
{
    private HealthController _healthController;

    [SerializeField]
    private ParticleSystem _particle;

    [SerializeField] [Space]
    private CFXR_Effect _cfxrEffect;





    private void Awake() => _healthController = Get<HealthController>.From(gameObject);

    private void OnEnable() => _healthController.onTankDeath += OnTankDeath;

    private void OnTankDeath()
    {
        transform.SetParent(null);

        StartCoroutine(PlayParticleAndToggleCameraShakeAfterDelay());
    }

    private IEnumerator PlayParticleAndToggleCameraShakeAfterDelay()
    {
        yield return new WaitForSeconds(1f);

        _cfxrEffect.enabled = true;

        _particle.Play(true);

        print("TankDeathExplosion");
    }
}
