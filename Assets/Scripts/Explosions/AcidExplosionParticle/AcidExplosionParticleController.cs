using System.Collections.Generic;
using UnityEngine;

public class AcidExplosionParticleController : MonoBehaviour
{
    public IScore OwnerScore { get; set; }

    [SerializeField]
    private ParticleSystem _particle;

    [SerializeField] [Space]
    private GameObject _parentGameobject;

    [SerializeField] [Space]
    private GameObject[] _bubbles;

    private GameObject _collidedGameobject;

    private List<ParticleCollisionEvent> _particleCollisionEvents = new List<ParticleCollisionEvent>();
    private List<Vector3> _collisionsIntersectionPoints = new List<Vector3>();
    private List<IDamage> _collidedDamagables = new List<IDamage>();

    private int _collisionsCount;
    private int _damage = 60;

    private bool _isBubblesActive;






    private void OnParticleCollision(GameObject other)
    {
        int collisionEventsNum = _particle.GetCollisionEvents(other, _particleCollisionEvents);

        bool hasRemainingBubbles = _collisionsCount < _bubbles.Length;

        if (hasRemainingBubbles)
        {
            StoreCollisionsIntersectionPoints();

            TryGetIDamagables();
        }
        else
        {
            ActivateBubbleCollisionEffect();
        }

        _collisionsCount++;
    }

    private void OnParticleSystemStopped() => DestroyParticles();

    private void StoreCollisionsIntersectionPoints() => _collisionsIntersectionPoints.Add(_particleCollisionEvents[0].intersection);

    private void TryGetIDamagables()
    {
        if (_collidedGameobject == _particleCollisionEvents[0].colliderComponent.gameObject)
            return;

        _collidedGameobject = _particleCollisionEvents[0].colliderComponent.gameObject;

        IDamage iDamage = Get<IDamage>.From(_collidedGameobject);

        if (iDamage == null)
            return;

        if (_collidedDamagables.Contains(iDamage))
            return;
        else
            StoreDamagablesApplyDamage(iDamage);
    }

    private void StoreDamagablesApplyDamage(IDamage iDamage)
    {
        _collidedDamagables.Add(iDamage);     
    }

    private void ApplyDamageAndScore(IDamage iDamage)
    {
        if (MyPhotonNetwork.IsOfflineMode)
            iDamage.Damage(_damage);
    }

    private void ActivateBubbleCollisionEffect()
    {
        if (_isBubblesActive)
            return;

        _isBubblesActive = true;

        for (int i = 0; i < _bubbles.Length; i++)
        {
            _bubbles[i].transform.SetParent(null);
            _bubbles[i].gameObject.SetActive(true);
            _bubbles[i].transform.position = _collisionsIntersectionPoints[i];
        }
    }

    private void DestroyParticles() => Destroy(_parentGameobject);
}
