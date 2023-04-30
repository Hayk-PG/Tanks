using System;
using UnityEngine;

public class TankMovement : BaseTankMovement
{
    private TankController _tankController;
    private Fuel _fuel;
    private IPhotonPlayerJumpExecuter _iPhotonPlayerJumpExecuter;

    private bool _isEnoughFuel = true;
    private bool _isSandbagsTriggered;

    [SerializeField] [Space] 
    private bool _isJumpButtonPressed;

    [SerializeField] 
    private bool _isJumped;

    private Action FixedUpdateFunction;
    private Action JumpFunction;

    public float SpeedInSandbags
    {
        get
        {
            if (_isSandbagsTriggered)
            {
                if (_playerTurn.MyTurn == TurnState.Player1 && Direction > 0 || _playerTurn.MyTurn == TurnState.Player2 && Direction < 0)
                    return 0;
                else
                    return 1;
            }
            else
                return 1;
        } 
    }

    public Vector3 SynchedPosition { get; set; }
    public Quaternion SynchedRotation { get; set; }




    protected override void Awake()
    {
        base.Awake();    
        
        RigidbodyCenterOfMass();

        GetTankController();

        GetIPhotonPlayerJumpExecuter();

        SynchVariables(false);
    }

    private void Start()
    {
        DefineFunctions();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        _tankController.OnMovementDirection += OnHorizontalJoystick;
        _tankController.onSelectJumpButton += OnJump;

        if (_fuel != null) 
            _fuel.OnFuelValue -= OnFuelValue;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        _tankController.OnMovementDirection -= OnHorizontalJoystick;
        _tankController.onSelectJumpButton -= OnJump;
    }

    private void FixedUpdate() => FixedUpdateFunction();

    private void GetTankController() => _tankController = Get<TankController>.From(gameObject);

    private void GetIPhotonPlayerJumpExecuter()
    {
        if (_tankController.BasePlayer == null)
            return;

        _iPhotonPlayerJumpExecuter = Get<IPhotonPlayerJumpExecuter>.From(_tankController.BasePlayer.gameObject);
    }

    private void DefineFunctions()
    {
        JumpFunction = MyPhotonNetwork.IsOfflineMode ? Jump : JumpRPC;

        FixedUpdateFunction = MyPhotonNetwork.IsOfflineMode ?

            delegate
            {
                Move();

                ControlJump();
            }:
            delegate
            {
                Move();

                ControlJump();

                SynchVariables(true);

                SynchTransformAndRotationOnNonLocalPlayer();
            };
    }

    private void OnHorizontalJoystick(float horizontal)
    {
        if (_playerTurn.IsMyTurn && _isEnoughFuel && !_isStunned)
            Direction = name == Names.Tank_FirstPlayer ? horizontal : -horizontal;

        else
            Direction = 0;
    }

    private void OnJump()
    {
        if (!_isJumpButtonPressed && !_isJumped && _wheelColliderController.IsPartiallyGrounded() && _playerTurn.IsMyTurn)
        {
            JumpFunction();

            _isJumpButtonPressed = true;
        }
    }

    public void Jump()
    {
        _rigidBody.AddForce(Vector3.up * 2500, ForceMode.Impulse);

        SecondarySoundController.PlaySound(7, 0);
    }

    private void JumpRPC()
    {
        Jump();

        _iPhotonPlayerJumpExecuter.Jump();
    }

    private void ControlJump()
    {
        if (_isJumpButtonPressed && !_wheelColliderController.IsGrounded())
        {           
            _isJumpButtonPressed = false;
            _isJumped = true;
        }

        if (_isJumped)
            _rigidBody.velocity += new Vector3(Direction, 0, 0) * 2.5f * Time.fixedDeltaTime;

        if (_wheelColliderController.IsGrounded() && _isJumped)
            _isJumped = false;
    }

    public void Move()
    {
        MotorTorque();

        Brake();

        RigidbodyEulerAngles();

        if (_playerTurn.IsMyTurn)
        {
            Raycasts();

            UpdateSpeedAndPush();

            RaiseOnVehicleMoveEvent(_wheelColliderController.WheelRPM());
        }
    }

    private void MotorTorque()
    {
        _wheelColliderController.MotorTorque(Direction * Speed * SpeedInSandbags * _speedBlocker * Time.fixedDeltaTime);
        _wheelColliderController.RotateWheels();

        OnDirectionValue?.Invoke(Direction);
        OnFuel?.Invoke((_wheelColliderController.WheelRPM() / 100) * fuelConsumptionPercent, _playerTurn.IsMyTurn);
        OnRigidbodyPosition?.Invoke(_rigidBody);       
    }

    private void RaiseOnVehicleMoveEvent(float value) => OnVehicleMove?.Invoke(Direction == 0 ? 0 : value);

    private void SynchVariables(bool onlyOnLocalPlayer)
    {
        if (onlyOnLocalPlayer && _tankController.BasePlayer == null)
            return;

        SynchedPosition = transform.position;
        SynchedRotation = transform.rotation;
    }

    private void SynchTransformAndRotationOnNonLocalPlayer()
    {
        if(_tankController.BasePlayer == null)
        {
            float distance = Vector3.Distance(transform.position, SynchedPosition);

            if(distance >= 1)
            {
                transform.position = SynchedPosition;
                transform.rotation = SynchedRotation;
            }
            else
            {
                float lerp = distance >= 0.5f ? 5 : 1;

                transform.position = Vector3.Lerp(transform.position, SynchedPosition, lerp * Time.fixedDeltaTime);
                transform.rotation = Quaternion.Lerp(transform.rotation, SynchedRotation, lerp * Time.fixedDeltaTime);
            }
        }
    }

    private void UpdateSpeedAndPush()
    {
        _isOnRightSlope = Direction > 0.5f && _rayCasts.FrontHit.collider?.name == _slopesNames[0];
        _isOnLeftSlope = Direction < -0.5f && _rayCasts.BackHit.collider?.name == _slopesNames[1];

        if (_isOnRightSlope) 
            RigidbodyPush(_vectorRight, 2500);

        if (_isOnLeftSlope) 
            RigidbodyPush(_vectorLeft, 2500);

        if (_isOnRightSlope || _isOnLeftSlope) 
            Speed = _accelerated;

        else 
            Speed = _normalSpeed;
    }

    private void Brake()
    {
        _currentBrake = IsVehicleStopped(Direction) ? _maxBrake : 0;

        _wheelColliderController.BrakeTorque(_currentBrake);
    }

    private void RigidbodyEulerAngles()
    {
        RotationXAxis = Converter.AngleConverter(_rigidBody.transform.eulerAngles.x) >= 65 || Converter.AngleConverter(_rigidBody.transform.eulerAngles.x) <= -65 ?
                        0 : _rigidBody.transform.eulerAngles.x;

        _rigidBody.transform.eulerAngles = new Vector3(_isJumped ? _rigidBody.transform.eulerAngles.x : RotationXAxis, InitialRotationYAxis, 0);
    }

    private void RigidbodyCenterOfMass()
    {
        _rigidBody.centerOfMass = _normalCenterOfMass;
    }

    private void RigidbodyPush(Vector3 direction, float force)
    {
        _rigidBody.AddForce(direction * force * Time.fixedDeltaTime, ForceMode.Impulse);
    }

    private void Raycasts()
    {
        _rayCasts.CastRays(_vectorRight, _vectorLeft);
    }

    private bool IsVehicleStopped(float value)
    {
        return value == 0;
    }

    internal void SubscribeToFuelEvents(Fuel fuel)
    {
        _fuel = fuel;
        _fuel.OnFuelValue += OnFuelValue;
    }

    private void OnFuelValue(bool isEnoughFuel)
    {
        _isEnoughFuel = isEnoughFuel;
    }

    public void OnEnteredSandbagsTrigger(bool isEntered)
    {
        _isSandbagsTriggered = isEntered;
    }

    protected override void OnAllowingPlayerToMoveOnlyFromLeftToRight(bool? mustMoveFromLeftToRightOnly)
    {
        if (mustMoveFromLeftToRightOnly.HasValue && mustMoveFromLeftToRightOnly.Value == true && _playerTurn.MyTurn == TurnState.Player2)
        {
            if (Direction > 0)
                _speedBlocker = 0;
            else
                _speedBlocker = 1;
        }

        if (mustMoveFromLeftToRightOnly.HasValue && mustMoveFromLeftToRightOnly.Value == false && _playerTurn.MyTurn == TurnState.Player1)
        {
            if (Direction > 0)
                _speedBlocker = 0;
            else
                _speedBlocker = 1;
        }

        if (!mustMoveFromLeftToRightOnly.HasValue)
            _speedBlocker = 1;
    }
}
