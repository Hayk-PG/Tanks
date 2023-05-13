using UnityEngine;
using CartoonFX;



public class MovementBoostEffectController : MonoBehaviour, IMovementBoostObserver
{
    [SerializeField]
    private ParticleSystem _particle;

    [Header("OPTIONAL")][SerializeField] [Space]
    private CFXR_Effect _cfxrEffect;





    public void SetMovementBoostActive(bool isMovementBoostActive)
    {
        ToggleCameraShake(isMovementBoostActive);

        if (isMovementBoostActive)
        {
            _particle.Play(true);

            return;
        }

        _particle.Stop(true);
    }

    // Shakes the camera upon particle activation.
    // Enables or disables the 'CFXR_Effect' component associated with the particle.

    private void ToggleCameraShake(bool isEnabled)
    {
        if (_cfxrEffect == null)
            return;

        _cfxrEffect.enabled = isEnabled;
    }
}
