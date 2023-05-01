using System.Collections.Generic;
using UnityEngine;

public class AcidExplosionParticleController : MonoBehaviour
{
    [SerializeField]
    private GameObject _parentGameobject;

    private Dictionary<IDamage, int> CollidedDamagables = new Dictionary<IDamage, int>();

    private int _collisionsCount;
    private int _damage;

    

    

    private void OnParticleCollision(GameObject other)
    {
        _collisionsCount++;

        if (_collisionsCount % 100 == 0)
        {
            _damage = _collisionsCount / 10;

            //print($"Damage: {_damage}");

            GetCollidedDamagables(other, _damage);
        }
    }

    private void OnParticleSystemStopped()
    {
        ApplyDamage();

        DestroyParticles();
    }

    private void GetCollidedDamagables(GameObject gameObject, int damage)
    {
        IDamage iDamage = Get<IDamage>.From(gameObject);

        if (iDamage == null)
            return;

        if (CollidedDamagables.ContainsKey(iDamage))
            CollidedDamagables[iDamage] += damage;
        else
            CollidedDamagables.Add(iDamage, damage);
    }

    private void ApplyDamage()
    {
        print($"CollidedDamagables: {CollidedDamagables.Count}");

        foreach (var collidedDamagable in CollidedDamagables)
        {
            collidedDamagable.Key.Damage(collidedDamagable.Value);

            print($"Damage: {collidedDamagable.Key}/{collidedDamagable.Value}");
        }
    }

    private void DestroyParticles() => Destroy(_parentGameobject);
}
