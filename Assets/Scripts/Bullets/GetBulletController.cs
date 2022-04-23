using UnityEngine;

public class GetBulletController : MonoBehaviour
{
    protected IBulletCollision _iBulletCollision;
    protected IBulletLimit _iBulletLimit;
    protected IBulletVelocity<BulletController.VelocityData> _iBulletVelocity;
    protected ITurnController _iTurnController;
    protected GameManagerBulletSerializer _gameManagerBulletSerializer;


    protected virtual void Awake()
    {
        _iBulletCollision = Get<IBulletCollision>.From(gameObject);
        _iBulletLimit = Get<IBulletLimit>.From(gameObject);
        _iBulletVelocity = Get<IBulletVelocity<BulletController.VelocityData>>.From(gameObject);
        _iTurnController = Get<ITurnController>.From(gameObject);
        _gameManagerBulletSerializer = FindObjectOfType<GameManagerBulletSerializer>();
    }
}
