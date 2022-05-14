using System;
using UnityEngine;

[Serializable]
public class BaseTankMovement : MonoBehaviour
{
    [Header("Movement parameters")]
    public float _normalSpeed;
    public float _accelerated;    
    public float _maxBrake;
    public Vector3 _normalCenterOfMass;
    protected float _currentBrake;

    [Header("Components")]
    public WheelColliderController _wheelColliderController;
    public Rigidbody _rigidBody;
    public Raycasts _rayCasts;
    protected PlayerTurn _playerTurn;

    protected bool _isOnRightSlope, _isOnLeftSlope;
    protected string[] _slopesNames;
    protected Vector3 _vectorRight;
    protected Vector3 _vectorLeft;

    public virtual float Speed { get; set; }
    public virtual float Direction { get; set; }     
    protected float InitialRotationYAxis { get; set; }

    internal Action<float> OnVehicleMove { get; set; }
    public Action<Rigidbody> OnRigidbodyPosition { get; set; }


    protected virtual void Awake()
    {
        _playerTurn = GetComponent<PlayerTurn>();

        Initialize();
    }

    void Initialize()
    {
        InitialRotationYAxis = _rigidBody.position.x < 0 ? 90 : -90;

        string right = InitialRotationYAxis > 0 ? "RS" : "LS";
        string left = InitialRotationYAxis > 0 ? "LS" : "RS";
        _slopesNames = new string[2] { right, left };

        _vectorRight = InitialRotationYAxis > 0 ? Vector3.right : Vector3.left;
        _vectorLeft = InitialRotationYAxis > 0 ? Vector3.left : Vector3.right;
    }
}
