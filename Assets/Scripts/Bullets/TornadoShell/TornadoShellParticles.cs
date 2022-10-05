using UnityEngine;

public class TornadoShellParticles : FlareBulletParticles
{
    protected override void OnCollision(RaycastHit hit, IScore ownerScore, float distance)
    {
        ActivateExplosion(hit);
        _explosion.OwnerScore = ownerScore;
        _bulletControllerWithRaycast.TurnController.SetNextTurn(TurnState.Transition);
        Destroy(gameObject);
    }
}
