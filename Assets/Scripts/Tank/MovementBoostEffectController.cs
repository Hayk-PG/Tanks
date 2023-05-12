using UnityEngine;

public class MovementBoostEffectController : MonoBehaviour, IMovementBoostObserver
{
    [SerializeField]
    private ParticleSystem _particle;



    public void SetMovementBoostActive(bool isMovementBoostActive)
    {
        if (isMovementBoostActive)
        {
            _particle.Play(true);

            return;
        }

        _particle.Stop(true);
    }
}
