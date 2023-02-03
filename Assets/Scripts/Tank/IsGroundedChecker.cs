using UnityEngine;

public class IsGroundedChecker : MonoBehaviour
{
    private WheelColliderController _wheelColliderController;
    private PlayerTurn _playerTurn;
    private TurnController _turnController;

    private float _notGroundedTime;




    private void Awake()
    {
        _wheelColliderController = Get<WheelColliderController>.FromChild(transform.parent.gameObject);
        _playerTurn = Get<PlayerTurn>.From(gameObject);
        _turnController = FindObjectOfType<TurnController>();
    }

    private void OnEnable()
    {
        _turnController.OnTurnChanged += (turnState) => 
        {
            if (turnState == _playerTurn.MyTurn)
            {
                _notGroundedTime = -0.5f;
            }
        };
    }

    private void OnDisable()
    {
        _turnController.OnTurnChanged -= (turnState) =>
        {
            if (turnState == _playerTurn.MyTurn)
            {
                _notGroundedTime = -0.5f;
            }
        };
    }

    private void FixedUpdate()
    {
        if(!_wheelColliderController.IsGrounded() && _notGroundedTime < 1)
        {
            _notGroundedTime += Time.deltaTime;
        }

        if(_notGroundedTime >= 1 && _wheelColliderController.IsGrounded())
        {
            SecondarySoundController.PlaySound(2, 0);
            _notGroundedTime = 0;
        }
    }
}
