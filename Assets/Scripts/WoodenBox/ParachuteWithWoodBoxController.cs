using System;
using System.Collections;
using UnityEngine;


public class ParachuteWithWoodBoxController : MonoBehaviour
{
    [SerializeField] private GameObject _parachute, _sparkles, _woodBoxExplosion;

    private ParachuteWithWoodBoxCollision _parachuteWithWoodBoxCollision;
    private BoxTrigger _boxTrigger;
    private WoodenBoxSerializer _woodenBoxSerializer;

    private delegate float Value();
    private Value _gravity;

    public Rigidbody RigidBody { get; set; }
    public float RandomDestroyTime { get; set; }


    private void Awake()
    {
        _parachuteWithWoodBoxCollision = Get<ParachuteWithWoodBoxCollision>.From(gameObject);
        _boxTrigger = Get<BoxTrigger>.FromChild(gameObject);
        _gravity = delegate { return 30 * Time.fixedDeltaTime; };
        _woodenBoxSerializer = FindObjectOfType<WoodenBoxSerializer>();
        RigidBody = Get<Rigidbody>.From(gameObject);

        Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, SetInitValues, AllocateWoodBoxController);
    }

    private void OnEnable()
    {
        _parachuteWithWoodBoxCollision.onCollisionEnter += OnCollision;
        _boxTrigger.OnBoxTriggerEntered += OnBoxTriggerEntered;
    }

    private void OnDisable()
    {
        _parachuteWithWoodBoxCollision.onCollisionEnter -= OnCollision;
        _boxTrigger.OnBoxTriggerEntered -= OnBoxTriggerEntered;
    }

    private void FixedUpdate() => Rigidbody();

    private void AllocateWoodBoxController() => _woodenBoxSerializer.AllocateWoodBoxController();

    public void SetInitValues()
    {
        RandomDestroyTime = UnityEngine.Random.Range(30, 90);
        StartCoroutine(DestroyAfterTime());
    }

    private void OnCollision(ParachuteWithWoodBoxCollision.CollisionData collisionData)
    {
        OnLand();

        if (collisionData._tankController != null || collisionData._bulletController != null)
        {
            DestroyGameobject();
        }      
    }

    private void OnBoxTriggerEntered()
    {
        DestroyGameobject();
    }

    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(RandomDestroyTime);
        DestroyGameobject();
    }

    private void Rigidbody()
    {
        RigidBody.velocity = new Vector3(RigidBody.velocity.x, RigidBody.velocity.y * _gravity(), 0);
        RigidBody.position = new Vector3(RigidBody.position.x, RigidBody.position.y, 0);
        RigidBody.rotation = Quaternion.Euler(0, 0, RigidBody.rotation.z);

        if (RigidBody.position.y <= -5)
            DestroyGameobject();
    }

    private void OnLand()
    {
        if (_parachute.activeInHierarchy)
        {
            _parachute.SetActive(false);
            _sparkles.SetActive(true);
            _gravity = delegate { return 1; };
        }
    }

    private void DestroyGameobject()
    {
        if (MyPhotonNetwork.IsOfflineMode)
        {
            Explosion();
            Destroy(gameObject);
        }
        else
        {
            _woodenBoxSerializer.DestroyParachuteWithWoodBoxController();
        }       
    }

    public void Explosion()
    {
        _woodBoxExplosion.SetActive(true);
        _woodBoxExplosion.transform.SetParent(null);
    }
}
