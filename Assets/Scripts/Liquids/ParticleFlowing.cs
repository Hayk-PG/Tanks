using UnityEngine;

public class ParticleFlowing : MonoBehaviour
{
    private IDamage _iDamage;
    private int _collisionsCount;


    private void OnParticleCollision(GameObject other)
    {
        _collisionsCount++;

        if (_collisionsCount % 4 == 0)
            Damage(other);
    }

    private void Damage(GameObject other)
    {
        _iDamage = Get<IDamage>.From(other);
        _iDamage?.Damage(5);
    }
}
