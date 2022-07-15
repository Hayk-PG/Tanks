using UnityEngine;

public class BulletRaycastReceiver : MonoBehaviour
{
    private BulletRaycasts _bulletRaycasts;
    private BulletControllerWithRaycast _bulletControllerWithRaycast;


    private void Awake()
    {
        _bulletRaycasts = Get<BulletRaycasts>.FromChild(gameObject);
        _bulletControllerWithRaycast = Get<BulletControllerWithRaycast>.From(gameObject);
    }

    private void OnEnable()
    {
        if (_bulletRaycasts != null) _bulletRaycasts._front.OnHit += OnFrontRaycastHit;
    }

    private void OnDisable()
    {
        if (_bulletRaycasts != null) _bulletRaycasts._front.OnHit -= OnFrontRaycastHit;
    }

    private void OnFrontRaycastHit(RaycastHit hit)
    {
        _bulletControllerWithRaycast.OnCollision?.Invoke(hit.collider, _bulletControllerWithRaycast.OwnerScore);
        _bulletControllerWithRaycast.OnExplodeOnCollision?.Invoke(_bulletControllerWithRaycast.OwnerScore);
    }
}
