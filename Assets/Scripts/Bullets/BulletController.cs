using System;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public Rigidbody RigidBody { get; private set; }

    [Serializable] struct Particles
    {
        [SerializeField] internal GameObject explosion;
    }
    [SerializeField] Particles _particles;



    void Awake()
    {
        RigidBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        RigidBody.transform.rotation = Quaternion.LookRotation(RigidBody.velocity);
    }

    void OnCollisionEnter(Collision collision)
    {
        OnCollisionWithIDamagables(collision);
        Explode();
        Destroy(gameObject);
    }

    void OnCollisionWithIDamagables(Collision collision)
    {
        collision.gameObject.GetComponent<IDamage>()?.Damage(0);
    }

    void Explode()
    {
        _particles.explosion.SetActive(true);
        _particles.explosion.transform.parent = null;
    }
}
