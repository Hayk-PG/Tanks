using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    [SerializeField] private GameObject _trail;
    [SerializeField] private GameObject _explosion;

    private Rigidbody _rigidBody;
    private GlobalExplosiveBarrels _globalExplosiveBarrels;
    private LavaSplash _lavaSplash;

    private bool _isActive;
    private Vector3 id;



    private void Awake()
    {
        _rigidBody = Get<Rigidbody>.From(gameObject);
        _globalExplosiveBarrels = FindObjectOfType<GlobalExplosiveBarrels>();
        _lavaSplash = FindObjectOfType<LavaSplash>();

        id = transform.position;
    }

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += Damage;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= Damage;
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
        Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode,
                delegate { Damage(collision.gameObject.tag, collision.transform.position); },
                delegate { _globalExplosiveBarrels.ExplodeBarrelRaiseEvent(collision.gameObject.tag, collision.transform.position, id); });
    }

    public void LaunchBarrel()
    {
        transform.SetParent(null);
        _isActive = true;
        _trail.SetActive(true);
        _rigidBody.isKinematic = false;
        _rigidBody.velocity = new Vector3(Random.Range(-1, 1), 1, 0) * Random.Range(3, 10);
        _rigidBody.rotation = Quaternion.LookRotation(_rigidBody.velocity);
        SecondarySoundController.PlaySound(2, 2);
    }

    private void Damage(string collisionTag, Vector3 collisionPosition)
    {
        if (!System.String.IsNullOrEmpty(collisionTag))
        {
            GameObject collision = collisionTag == Tags.AI || collisionTag == Tags.Player ?
                               GlobalFunctions.ObjectsOfType<TankController>.Find(tank => tank.transform.position == collisionPosition)?.gameObject :
                               collisionTag == Tags.Tile ? GlobalFunctions.ObjectsOfType<Tile>.Find(tile => tile.transform.position == collisionPosition)?.gameObject : null;

            if (collision != null)
            {
                Get<IDestruct>.From(collision.gameObject)?.Destruct(27, 0);
                Get<IDamage>.From(collision.gameObject)?.Damage(32);
            }
        }

        Explode(true);
    }

    private void Damage(EventData eventData)
    {
        if(eventData.Code == EventInfo.Code_BarrelCollision)
        {
            object[] data = (object[])eventData.CustomData;

            if ((Vector3)data[2] == id)
                Damage((string)data[0], (Vector3)data[1]);
        }
    }

    private void Explode(bool includeExplosion)
    {
        _explosion.transform.SetParent(null);
        _explosion.SetActive(includeExplosion);
        ExplosionsSoundController.PlaySound(1, 3);
        Destroy(gameObject);
    }
}