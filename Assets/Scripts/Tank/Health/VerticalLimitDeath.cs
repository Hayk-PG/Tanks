using UnityEngine;

public class VerticalLimitDeath : MonoBehaviour
{
    private BaseTankMovement _baseTankMovement;
    private IDamage _iDamage;
    private LavaSplash _lavaSplash;

    private bool _hasFallenIntoLavaLiquid;




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
        if (_hasFallenIntoLavaLiquid)
            return;

        if (IsBelowMinVerticalLimit(rigidbody.position.y))
        {
            _lavaSplash.ActivateLargeSplash(rigidbody.position);

            _iDamage.Damage(1000);

            SetTankLayerToDisappear();

            _hasFallenIntoLavaLiquid = true;
        }
    }

    private bool IsBelowMinVerticalLimit(float verticalPosition)
    {
        return verticalPosition <= VerticalLimit.Min;
    }

    private void SetTankLayerToDisappear()
    {
        GlobalFunctions.Loop<Transform>.Foreach(GetComponentsInChildren<Transform>(), child => child.gameObject.layer = 12);
    }
}
