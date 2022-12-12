using UnityEngine;

public class Barrel : MonoBehaviour
{
    private BoxCollider _boxCollider;
    private Rigidbody _rigidBody;
    private GlobalExplosiveBarrels _globalExplosiveBarrels;


    private void Awake()
    {
        _boxCollider = Get<BoxCollider>.From(gameObject);
        _rigidBody = Get<Rigidbody>.From(gameObject);
        _globalExplosiveBarrels = FindObjectOfType<GlobalExplosiveBarrels>();
    }

    public void LaunchBarrel()
    {
        transform.SetParent(null);
        _rigidBody.isKinematic = false;
        _rigidBody.velocity = new Vector3(-0.3f, 1, 0) * 5;
        _rigidBody.rotation = Quaternion.LookRotation(_rigidBody.velocity);
        _globalExplosiveBarrels.AllocateBarrel(_rigidBody.position);
    }
}