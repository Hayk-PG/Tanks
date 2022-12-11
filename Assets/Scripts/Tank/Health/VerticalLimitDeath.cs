using UnityEngine;

public class VerticalLimitDeath : MonoBehaviour
{
    private BaseTankMovement _baseTankMovement;
    private IDamage _iDamage;
    private LavaSplash _lavaSplash;


    private void Awake()
    {
        _baseTankMovement = Get<BaseTankMovement>.From(gameObject);
        _iDamage = Get<IDamage>.From(gameObject);
        _lavaSplash = FindObjectOfType<LavaSplash>();
    }

    private void OnEnable()
    {
        if (_baseTankMovement != null)
            _baseTankMovement.OnRigidbodyPosition += GetRigidBodyPosition;
    }

    private void OnDisable()
    {
        if (_baseTankMovement != null)
            _baseTankMovement.OnRigidbodyPosition -= GetRigidBodyPosition; 
    }

    private void GetRigidBodyPosition(Rigidbody rigidbody)
    {
        if (rigidbody.position.y <= VerticalLimit.Min && _iDamage.Health > 0)
        {
            _lavaSplash.ActivateLargeSplash(rigidbody.position);
            _iDamage.Damage(1000);
        }
    }
}
