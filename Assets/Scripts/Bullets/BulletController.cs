using UnityEngine;

public class BulletController : MonoBehaviour
{
    public Rigidbody RigidBody { get; private set; }


    void Awake()
    {
        RigidBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        RigidBody.transform.rotation = Quaternion.LookRotation(RigidBody.velocity);
    }
}
