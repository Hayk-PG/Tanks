using UnityEngine;

public class TankHealParticle : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _particle;

    [SerializeField] [Space]
    private CameraShaker _cameraShaker;

    private HealthController _healthController;



    private void Awake() => GetHealthController();

    private void OnEnable() => _healthController.onHealthBoost += OnHealthBoost;

    private void GetHealthController() => _healthController = Get<HealthController>.From(gameObject);

    private void OnHealthBoost() => PlayParticleAndToggleCameraShake();

    private void PlayParticleAndToggleCameraShake()
    {
        _particle.Play(true);

        _cameraShaker.ToggleShake();
    }
}
