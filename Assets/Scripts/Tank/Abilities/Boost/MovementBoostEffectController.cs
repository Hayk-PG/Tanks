using UnityEngine;

public class MovementBoostEffectController : MonoBehaviour, IMovementBoostObserver
{
    [SerializeField]
    private ParticleSystem _particle;




    public void SetMovementBoostActive(bool isMovementBoostActive)
    {
        if (isMovementBoostActive)
            _particle.Play(true);
        else
            _particle.Stop(true);
    }
}
