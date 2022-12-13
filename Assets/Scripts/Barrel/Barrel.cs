using UnityEngine;

public class Barrel : MonoBehaviour
{
    [SerializeField] private GameObject _trail;
    [SerializeField] private GameObject _explosion;

    private Rigidbody _rigidBody;
    private GlobalExplosiveBarrels _globalExplosiveBarrels;
    private LavaSplash _lavaSplash;

    private bool _isActive;
    private int _collisionsCount;

    public Rigidbody Rigidbody => _rigidBody;


    private void Awake()
    {
        _rigidBody = Get<Rigidbody>.From(gameObject);
        _globalExplosiveBarrels = FindObjectOfType<GlobalExplosiveBarrels>();
        _lavaSplash = FindObjectOfType<LavaSplash>();
    }

    private void FixedUpdate()
    {
        if (_isActive && _rigidBody.position.y <= VerticalLimit.Min)
        {
            _lavaSplash.ActivateSmallSplash(_rigidBody.position);
            Explode(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        _collisionsCount++;

        Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode,
                delegate { Damage(collision.gameObject.tag, collision.transform.position); },
                delegate { _globalExplosiveBarrels.ExplodeBarrel(collision.gameObject.name, collision.transform.position, _rigidBody.position); });
    }

    public void LaunchBarrel()
    {
        transform.SetParent(null);
        _isActive = true;
        _trail.SetActive(true);
        _rigidBody.isKinematic = false;
        _rigidBody.velocity = new Vector3(Random.Range(-1, 1), 1, 0) * Random.Range(3, 10);
        _rigidBody.rotation = Quaternion.LookRotation(_rigidBody.velocity);
        _globalExplosiveBarrels.AllocateBarrel(_rigidBody.position);
        SecondarySoundController.PlaySound(2, 2);
    }

    public void Damage(string collisionTag, Vector3 collisionPosition)
    {
        GameObject collision = collisionTag == Tags.AI || collisionTag == Tags.Player ? 
                               GlobalFunctions.ObjectsOfType<TankController>.Find(tank => tank.transform.position == collisionPosition).gameObject :
                               collisionTag == Tags.Tile ? GlobalFunctions.ObjectsOfType<Tile>.Find(tile => tile.transform.position == collisionPosition).gameObject : null;

        if (collision != null)
        {
            Get<IDestruct>.From(collision.gameObject)?.Destruct(27, 0);
            Get<IDamage>.From(collision.gameObject)?.Damage(32);
        }

        Explode(true);
    }

    private void Explode(bool includeExplosion)
    {
        _explosion.transform.SetParent(null);
        _explosion.SetActive(includeExplosion);
        ExplosionsSoundController.PlaySound(1, 3);
        Destroy(gameObject);
    }
}