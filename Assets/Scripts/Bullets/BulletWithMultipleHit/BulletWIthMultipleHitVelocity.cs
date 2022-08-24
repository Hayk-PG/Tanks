using UnityEngine;

public class BulletWIthMultipleHitVelocity : BulletVelocity
{
    protected BulletWithMultipleHitController _bulletWithMultipleHitController;
    protected BulletRaycasts _bulletRaycasts;
    protected Vector3? _direction;
    protected int _force;


    protected override void Awake()
    {
        base.Awake();

        _bulletWithMultipleHitController = Get<BulletWithMultipleHitController>.From(gameObject);
        _bulletRaycasts = Get<BulletRaycasts>.FromChild(gameObject);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        _bulletWithMultipleHitController.OnCollision += OnCollision;
        _bulletRaycasts._front.OnHit += OnHit;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        _bulletWithMultipleHitController.OnCollision -= OnCollision;
        _bulletRaycasts._front.OnHit -= OnHit;
    }

    protected virtual void OnCollision(Collider collider, IScore iScore, float distance)
    {
        _bulletWithMultipleHitController.RigidBody.velocity = _direction == null ? Vector3.up : _direction.Value * (10 - _force);
        _force += 2;
    }

    protected virtual void OnHit(RaycastHit hit)
    {
        _direction = hit.normal;
    }
}
