using System.Collections.Generic;
using UnityEngine;

public class AcidExplosionParticleController : MonoBehaviour
{
    public IScore OwnerScore { get; set; }
    public IDamage IDamageAi { get; set; }

    [SerializeField]
    private ParticleSystem _particle;

    [SerializeField] [Space]
    private GameObject[] _bubbles;

    private GameObject _collidedGameobject;

    private List<ParticleCollisionEvent> _particleCollisionEvents = new List<ParticleCollisionEvent>();
    private List<Vector3> _collisionsIntersectionPoints = new List<Vector3>();
    private List<IDamage> _collidedDamagables = new List<IDamage>();

    private int _collisionsCount;

    private bool _isBubblesActive;

    private int DamageValue => 60;
    private int ScoreValue => Random.Range(500, 1500);





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
        {
            StoreDamagables(iDamage);

            ApplyDamageAndScore(iDamage);
        }
    }

    private void StoreDamagables(IDamage iDamage) => _collidedDamagables.Add(iDamage);

    private void ApplyDamageAndScore(IDamage iDamage)
    {
        if (MyPhotonNetwork.IsOfflineMode)
        {
            iDamage.Damage(DamageValue);

            OwnerScore.GetScore(ScoreValue, iDamage, transform.position);
            OwnerScore.HitEnemyAndGetScore(new int[] { ScoreValue }, iDamage);
        }
        else
        {
            GameSceneObjectsReferences.GameManagerBulletSerializer.CallDamageAndScoreRPC(iDamage, OwnerScore, DamageValue, new int[] { ScoreValue }, 0);
        }
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
}
