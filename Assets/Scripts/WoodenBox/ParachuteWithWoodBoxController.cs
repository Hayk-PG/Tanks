using System;
using System.Collections;
using UnityEngine;


public class ParachuteWithWoodBoxController : MonoBehaviour
{
    [SerializeField]
    private ParachuteWithWoodBoxCollision _parachuteWithWoodBoxCollision;

    [SerializeField] [Space]
    private Rigidbody _rigidbody;

    private bool _isOutOfBounds;

    private delegate float Value();
    private Value _gravity;

    private Action DestroyGameobjectFunction;

    internal GameObject ParachuteObj { get; set; }

    public Rigidbody RigidBody => _rigidbody;
    public Vector3 Id { get; set; }
    public float RandomDestroyTime { get; set; }





    private void Awake()
    {
        _gravity = delegate { return 30 * Time.fixedDeltaTime; };

        DestroyGameobjectFunction = MyPhotonNetwork.IsOfflineMode ? DestroyGameobject : DestroyGameobjectRPC;
    }

    private void Start()
    {
        DestroyAfterDelay();

        GameSceneObjectsReferences.ParachuteIcon.Init(transform);
    }

    private void OnEnable() => _parachuteWithWoodBoxCollision.onCollisionEnter += OnCollision;

    private void OnDisable() => _parachuteWithWoodBoxCollision.onCollisionEnter -= OnCollision;

    private void FixedUpdate()
    {
        ControlRigidbodyMovement();

        DetectOutOfBounds();
    }

    private void DestroyAfterDelay() => StartCoroutine(DestroyAfterTime());

    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(RandomDestroyTime);

        DestroyGameobjectFunction();
    }

    private void OnCollision(ParachuteWithWoodBoxCollision.CollisionData collisionData)
    {
        OnLand();

        DestroyOnCollision(collisionData);
    }

    private void DestroyOnCollision(ParachuteWithWoodBoxCollision.CollisionData collisionData)
    {
        if (collisionData._tankController != null || collisionData._bulletController != null)
            DestroyGameobjectFunction();
    }

    private void ControlRigidbodyMovement()
    {
        RigidBody.velocity = new Vector3(RigidBody.velocity.x, RigidBody.velocity.y * _gravity(), 0);
        RigidBody.position = new Vector3(RigidBody.position.x, RigidBody.position.y, 0);
        RigidBody.rotation = Quaternion.Euler(0, 0, RigidBody.rotation.z);
    }

    private void DetectOutOfBounds()
    {
        if (RigidBody.position.y <= VerticalLimit.Min && !_isOutOfBounds)
        {
            GameSceneObjectsReferences.LavaSplash.ActivateSmallSplash(RigidBody.position);

            DestroyGameobjectFunction();

            _isOutOfBounds = true;
        }
    }

    private void OnLand()
    {
        if (ParachuteObj == null)
            return;

        if (ParachuteObj.activeInHierarchy)
        {
            HideParachute();

            EnhanceGravity();
        }
    }

    private void HideParachute() => ParachuteObj.SetActive(false);

    private void EnhanceGravity() => _gravity = delegate { return 1; };

    public void DestroyGameobject()
    {
        SecondarySoundController.PlaySound(9, 1);

        Destroy(gameObject);
    }

    private void DestroyGameobjectRPC()
    {
        GameSceneObjectsReferences.WoodBoxSerializer.DestroyParachuteWithWoodBoxController(Id);
    }
}
