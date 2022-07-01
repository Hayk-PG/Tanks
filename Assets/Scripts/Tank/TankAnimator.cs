using UnityEngine;

public class TankAnimator : MonoBehaviour
{
    private BaseShootController _baseShootController;
    private BaseTankMovement _baseTankMovement;
    private Animator _bodyAnimator;

    private bool _isApplyingForce;
    private bool _isMoving;


    private void Awake()
    {
        _baseShootController = Get<BaseShootController>.From(gameObject);
        _baseTankMovement = Get<BaseTankMovement>.From(gameObject);
        _bodyAnimator = Get<Animator>.From(transform.Find("Body").gameObject);
    }

    private void OnEnable()
    {
        if (_bodyAnimator != null)
        {
            _baseShootController.OnApplyingForce += OnApplyingForce;
            _baseTankMovement.OnVehicleMove += OnVehicleMove;
        }
    }

    private void OnDisable()
    {
        _baseShootController.OnApplyingForce -= OnApplyingForce;
        _baseTankMovement.OnVehicleMove -= OnVehicleMove;
    }

    private void OnApplyingForce(bool isApplyingForce)
    {
        _isApplyingForce = isApplyingForce;
        
        if (!_isMoving)
            _bodyAnimator.SetBool("isApplyingForce", _isApplyingForce);
    }

    private void OnVehicleMove(float rpm)
    {
        _isMoving = rpm != 0;

        if (!_isApplyingForce)
            _bodyAnimator.SetBool("isApplyingForce", _isMoving);
    }
}
