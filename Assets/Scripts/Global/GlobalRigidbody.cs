using UnityEngine;

public class GlobalRigidbody : MonoBehaviour
{
    private Rigidbody _rigidbody;


    private void Awake()
    {
        _rigidbody = Get<Rigidbody>.From(gameObject);
        _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
    }
}
