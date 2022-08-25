using UnityEngine;

public class BulletRaycastReceiver : MonoBehaviour
{
    protected BulletRaycasts _bulletRaycasts;
    protected BulletControllerWithRaycast _bulletControllerWithRaycast;


    protected virtual void Awake()
    {
        _bulletRaycasts = Get<BulletRaycasts>.FromChild(gameObject);
        _bulletControllerWithRaycast = Get<BulletControllerWithRaycast>.From(gameObject);
    }

    protected virtual void OnEnable()
    {
        if (_bulletRaycasts != null) _bulletRaycasts._front.OnHit += OnFrontRaycastHit;
    }

    protected virtual void OnDisable()
    {
        if (_bulletRaycasts != null) _bulletRaycasts._front.OnHit -= OnFrontRaycastHit;
    }

    protected virtual void OnFrontRaycastHit(RaycastHit hit)
    {
        _bulletControllerWithRaycast.OnCollision?.Invoke(hit.collider, _bulletControllerWithRaycast.OwnerScore, _bulletControllerWithRaycast.Distance);
        _bulletControllerWithRaycast.OnExplodeOnCollision?.Invoke(_bulletControllerWithRaycast.OwnerScore, _bulletControllerWithRaycast.Distance);
    }
}
