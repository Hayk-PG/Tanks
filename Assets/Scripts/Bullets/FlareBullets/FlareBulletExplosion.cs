using UnityEngine;

public class FlareBulletExplosion : BulletExplosion
{
    protected override void OnExplodeOnCollision(IScore ownerScore, float distance)
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 0.11f))
            OnFlareBulletExplosion?.Invoke(ownerScore, distance, hit.point);
        else
            OnFlareBulletExplosion?.Invoke(ownerScore, distance, null);

        Destroy(gameObject);
    }
}
