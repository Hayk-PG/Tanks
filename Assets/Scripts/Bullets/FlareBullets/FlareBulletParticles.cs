using System;
using UnityEngine;

public class FlareBulletParticles : BulletParticles
{
    protected BulletRaycastReceiver2 _bulletRaycastReceiver2;
    protected BulletControllerWithRaycast _bulletControllerWithRaycast;
    public Action<IScore, PlayerTurn, Vector3> OnFlareSignal { get; set; }


    protected override void Awake()
    {
        base.Awake();
        _bulletRaycastReceiver2 = Get<BulletRaycastReceiver2>.From(gameObject);
        _bulletControllerWithRaycast = Get<BulletControllerWithRaycast>.From(gameObject);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        if (_bulletRaycastReceiver2 != null)
            _bulletRaycastReceiver2.OnCollision += OnCollision;
    }

    protected override void OnDisable()
    {
        base.OnEnable();

        if (_bulletRaycastReceiver2 != null)
            _bulletRaycastReceiver2.OnCollision -= OnCollision;
    }

    protected virtual void ActivateExplosion(RaycastHit hit)
    {
        _explosion.gameObject.SetActive(true);
        _explosion.transform.parent = null;
        _explosion.transform.rotation = Quaternion.LookRotation(hit.normal);
        _explosion.transform.position = hit.point;
    }

    protected virtual void OnCollision(RaycastHit hit, IScore ownerScore, float distance)
    {
        PlayerTurn ownerTurn = GlobalFunctions.ObjectsOfType<PlayerTurn>.Find(turn => Get<IScore>.From(turn.gameObject) == ownerScore);
        ActivateExplosion(hit);
        OnFlareSignal?.Invoke(ownerScore, ownerTurn, _explosion.transform.position);
        Destroy(gameObject);
    }
}
