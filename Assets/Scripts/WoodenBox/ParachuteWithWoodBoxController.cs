using System;
using System.Collections;
using UnityEngine;


public class ParachuteWithWoodBoxController : MonoBehaviour
{
    [SerializeField]
    private ParachuteWithWoodBoxCollision _parachuteWithWoodBoxCollision;

    [SerializeField] [Space]
    private Rigidbody _rigidbody;

    [SerializeField] [Space]
    private BoxTrigger _boxTrigger;

    [SerializeField] [Space]
    private GameObject _sparkles, _woodBoxExplosion;

    private delegate float Value();
    private Value _gravity;

    internal GameObject ParachuteObj { get; set; }
    public Rigidbody RigidBody => _rigidbody;
    public float RandomDestroyTime { get; set; }




    private void Awake()
    {
        _gravity = delegate { return 30 * Time.fixedDeltaTime; };

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

    private void AllocateWoodBoxController() => GameSceneObjectsReferences.WoodBoxSerializer.AllocateWoodBoxController();

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

        if (RigidBody.position.y <= VerticalLimit.Min)
        {
            GameSceneObjectsReferences.LavaSplash.ActivateSmallSplash(RigidBody.position);
            DestroyGameobject();
        }
    }

    private void OnLand()
    {
        if (ParachuteObj == null)
            return;

        if (ParachuteObj.activeInHierarchy)
        {
            ParachuteObj.SetActive(false);
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
            GameSceneObjectsReferences.WoodBoxSerializer.DestroyParachuteWithWoodBoxController();
        }       
    }

    public void Explosion()
    {
        _woodBoxExplosion.SetActive(true);
        _woodBoxExplosion.transform.SetParent(null);
    }
}
