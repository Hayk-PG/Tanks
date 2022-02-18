using System;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public Rigidbody RigidBody { get; private set; }
    public IScore OwnerScore { get; set; }

    private TurnController _turnController;
    private CameraShake _cameraShake;
    private WindSystemController _windSystemController;

    [Serializable]
    private struct Particles
    {
        [SerializeField] internal GameObject _trail;      
        [SerializeField] internal GameObject _muzzleFlash;
        [SerializeField] internal Explosion explosion;
    }

    [SerializeField]
    private Particles _particles;

    private bool _isWindActivated;
    private int _collisionCount;


    private void Awake()
    {
        RigidBody = GetComponent<Rigidbody>();
        _turnController = FindObjectOfType<TurnController>();
        _cameraShake = FindObjectOfType<CameraShake>();
        _windSystemController = FindObjectOfType<WindSystemController>();

        _particles._muzzleFlash.transform.parent = null;

        Invoke("ActivateTrail", 0.1f);
        Invoke("ActivateWindForce", 0.5f);
    }

    private void Start()
    {
        _turnController.SetNextTurn(TurnState.Other);
    }
   
    private void OnEnable()
    {
        _turnController.OnTurnChanged += OnTurnChanged;
    }
    
    private void OnDisable()
    {
        _turnController.OnTurnChanged -= OnTurnChanged;
    }

    private void FixedUpdate()
    {
        DestroyBulletByPosition();

        RigidBody.transform.rotation = Quaternion.LookRotation(RigidBody.velocity);
        if(_isWindActivated) RigidBody.velocity += new Vector3(_windSystemController.WindForce * Time.fixedDeltaTime, 0, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnCollisionWithDestructables(collision);       
        DestroyBullet();
    }

    private void ActivateTrail()
    {
        _particles._trail.SetActive(true);
    }

    private void ActivateWindForce()
    {
        _isWindActivated = true;
    }

    private void OnTurnChanged(TurnState arg1, CameraMovement arg2)
    {
        if(arg1 == TurnState.Other)
        {
            arg2.SetCameraTarget(transform, 3);
        }
    }

    private void OnCollisionWithDestructables(Collision collision)
    {
        _collisionCount++;

        if (_collisionCount <= 1) Get<IDestruct>.From(collision.gameObject)?.Destruct();
    }

    private void Explode()
    {
        _particles.explosion.OwnerScore = OwnerScore;
        _particles.explosion.gameObject.SetActive(true);
        _particles.explosion.transform.parent = null;
        _cameraShake.Shake();
    }

    private void DestroyBulletByPosition()
    {
        if(RigidBody.position.y < -5)
        {
            DestroyBullet();
        }
    }

    private void DestroyBullet()
    {
        Explode();
        _turnController.SetNextTurn(TurnState.Transition);
        Destroy(gameObject);
    }
}
