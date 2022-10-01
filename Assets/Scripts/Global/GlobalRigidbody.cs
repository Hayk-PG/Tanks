using UnityEngine;

public class GlobalRigidbody : MonoBehaviour
{
    public Rigidbody RigidBody { get; private set; }


    private void Awake()
    {
        RigidBody = Get<Rigidbody>.From(gameObject);
        RigidBody.interpolation = RigidbodyInterpolation.Interpolate;
    }
}
