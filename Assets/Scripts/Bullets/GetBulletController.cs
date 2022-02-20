using UnityEngine;

public class GetBulletController : MonoBehaviour
{
    protected BulletController _bulletController;


    protected virtual void Awake()
    {
        _bulletController = Get<BulletController>.From(gameObject);
    }
}
