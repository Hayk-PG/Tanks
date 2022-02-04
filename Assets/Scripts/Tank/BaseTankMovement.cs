using UnityEngine;

public class BaseTankMovement : MonoBehaviour
{
    [SerializeField] protected float _normalSpeed, _accelerated;
    [SerializeField] protected float _currentBrake, _maxBrake;

    [SerializeField] protected Vector3 _normalCenterOfMass;

    [SerializeField] protected WheelColliderController _wheelColliderController;
    [SerializeField] protected Rigidbody _rigidBody;
    [SerializeField] protected Raycasts _rayCasts;

    protected bool _isOnRightSlope, _isOnLeftSlope;
    protected string[] _slopesNames;
    protected Vector3 _vectorRight;
    protected Vector3 _vectorLeft;

    public virtual float Speed { get; set; }
    public virtual float Direction { get; set; }     
    protected float InitialRotationYAxis { get; set; }


    protected virtual void Awake()
    {
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
