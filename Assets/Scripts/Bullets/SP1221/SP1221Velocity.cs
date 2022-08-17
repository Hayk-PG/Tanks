using UnityEngine;

public class SP1221Velocity : BulletVelocity
{
    private Rigidbody _rigidbody;
    private BulletRaycasts _bulletRaycast;

    private Vector3 _initialVelocity;
    private Vector3 _hitPoint;
    private bool _isFirstStagePassed;
    private bool _isSecondStagePassed;
    private bool _isVelocityResetted;
    private bool _isHitPointSet;
    private bool _isFallingSoundfxPlayed;
    private float _x, _y;
    private float _angle;
    private float _radius = 0.5f;
    private float _angularSpeed = 5;

    protected override void Awake()
    {
        base.Awake();
        _rigidbody = Get<Rigidbody>.From(gameObject);
        _bulletRaycast = Get<BulletRaycasts>.FromChild(gameObject);      
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        _bulletRaycast._front.OnHit += OnHit;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        _bulletRaycast._front.OnHit -= OnHit;
    }

    protected override void Start()
    {
        base.Start();
        _initialVelocity = _rigidbody.velocity;
    }

    private void OnHit(RaycastHit hit)
    {
        if (!_isHitPointSet)
        {
            _hitPoint = hit.point;
            _isHitPointSet = true;
        }
    }

    protected override void OnBulletVelocity(BulletController.VelocityData velocityData)
    {
        base.OnBulletVelocity(velocityData);

        if (velocityData._rigidBody.velocity.y < 0 && !_isFallingSoundfxPlayed)
            _isFallingSoundfxPlayed = true;

        if (_isHitPointSet)
            _isFirstStagePassed = true;

        if (_isFirstStagePassed && !_isVelocityResetted)
        {
            _angle += Time.deltaTime * 15;
            _x = Mathf.Cos(_angle) * _radius * 15;
            _y = Mathf.Sin(_angle) * _radius * 15;
            velocityData._rigidBody.velocity = new Vector3(_x, _y, velocityData._rigidBody.velocity.z);

            if(velocityData._rigidBody.velocity.y < 0 && velocityData._rigidBody.velocity.x < 0 && !_isSecondStagePassed)
            {
                
                _isSecondStagePassed = true;
            }

            if(_isSecondStagePassed && velocityData._rigidBody.velocity.y > 1 && velocityData._rigidBody.velocity.x > 1)
            {
                velocityData._rigidBody.velocity = Vector3.Lerp(velocityData._rigidBody.velocity, _initialVelocity, 2 * Time.fixedDeltaTime);
                
                _isVelocityResetted = true;
            }
        }

        //if (_isHitPointSet && _isVelocityResetted)
        //{
        //    velocityData._rigidBody.position = Vector3.Lerp(velocityData._rigidBody.position, _hitPoint, 2 * Time.fixedDeltaTime);
        //}
    }
}
