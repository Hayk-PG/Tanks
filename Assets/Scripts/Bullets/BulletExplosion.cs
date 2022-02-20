using UnityEngine;

public class BulletExplosion : GetBulletController
{
    [SerializeField]
    private Explosion _explosion;

    private CameraShake _cameraShake;


    protected override void Awake()
    {
        base.Awake();

        _cameraShake = FindObjectOfType<CameraShake>();
    }

    private void OnEnable()
    {
        if (_bulletController != null) _bulletController.OnExplodeOnCollision = OnExplodeOnCollision;
        if (_bulletController != null) _bulletController.OnExplodeOnLimit = OnExplodeOnLimit;
    }

    private void OnExplodeOnCollision(IScore ownerScore)
    {
        _explosion.OwnerScore = ownerScore;
        _explosion.gameObject.SetActive(true);
        _explosion.transform.parent = null;
        _cameraShake.Shake();
        DestroyBullet();
    }

    private void OnExplodeOnLimit(bool isTrue)
    {
        if(isTrue) DestroyBullet();
    }

    private void DestroyBullet()
    {
        _bulletController._turnController.SetNextTurn(TurnState.Transition);
        Destroy(gameObject);
    }
}
