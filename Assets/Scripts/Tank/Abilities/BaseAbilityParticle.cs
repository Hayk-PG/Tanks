using UnityEngine;


[RequireComponent(typeof(CameraShaker))]
public class BaseAbilityParticle<T> : MonoBehaviour where T: BaseAbility
{
    [SerializeField]
    protected ParticleSystem _particle;

    [SerializeField] [Space]
    protected CameraShaker _cameraShaker;

    protected T _ability;

    [SerializeField] [Space]
    protected int _clipIndex;





    protected virtual void Awake() => GetAbility();

    protected virtual void OnEnable() => _ability.onAbilityActive += OnAbilityActive;

    protected virtual void GetAbility() => _ability = Get<T>.From(gameObject);

    protected virtual void OnAbilityActive(bool isAbilityActive)
    {
        if (isAbilityActive)
            PlayParticle();
        else
            StopParticle();

        PlaySoundEffect();
    }

    protected virtual void PlayParticle()
    {
        _particle.Play(true);

        ToggleCameraShake();
    }

    protected virtual void StopParticle() => _particle.Stop(true);

    protected virtual void ToggleCameraShake()
    {
        if (_cameraShaker == null)
            return;

        _cameraShaker.ToggleShake();
    }

    protected virtual void PlaySoundEffect() => SecondarySoundController.PlaySound(10, _clipIndex);
}
