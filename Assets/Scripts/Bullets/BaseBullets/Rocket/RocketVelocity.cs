using UnityEngine;

public class RocketVelocity : BaseBulletVelocity
{
    [SerializeField] [Space]
    protected float _movementSpeed, _rotationSpeed;

    [SerializeField] [Space]
    protected RocketController _rocketController;

    protected float _rotationX;
    protected float _rotationY;   

    protected Quaternion _rotation;

    protected float Vertical
    {
        get => GameSceneObjectsReferences.TabRocketController.Direction.y;
    }
    protected float Horizontal
    {
        get => GameSceneObjectsReferences.TabRocketController.Direction.x == 0 ? 0 : _rotationY > 0 ? GameSceneObjectsReferences.TabRocketController.Direction.x :
               -GameSceneObjectsReferences.TabRocketController.Direction.x;
    }




    private void Awake() => _rotationY = Converter.AngleConverter(_baseBulletController.RigidBody.transform.eulerAngles.y);

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        SynchVariables();

        SycnhTransform();
    }

    protected override void ControlLookRotation()
    {
        _rotationX = Mathf.Atan2(Vertical, Horizontal) * Mathf.Rad2Deg;

        _rotation = Quaternion.Euler(-_rotationX, _rotationY, 0);

        _baseBulletController.RigidBody.rotation = Quaternion.Lerp(_baseBulletController.RigidBody.rotation, _rotation, _rotationSpeed * Time.fixedDeltaTime);
    }

    protected override void ControlMovement()
    {
        _baseBulletController.RigidBody.velocity = _baseBulletController.RigidBody.transform.forward * (_movementSpeed + Horizontal) * Time.fixedDeltaTime;
    }

    protected virtual void SynchVariables()
    {
        if (_rocketController?.Owner.BasePlayer == null)
            return;

        _rocketController.SynchedPosition = transform.position;
        _rocketController.SynchedRotation = transform.rotation;
    }

    protected virtual void SycnhTransform()
    {
        if (_rocketController?.Owner.BasePlayer != null)
            return;

        float distance = Vector3.Distance(transform.position, _rocketController.SynchedPosition);

        if (distance >= 1)
        {
            transform.position = _rocketController.SynchedPosition;
            transform.rotation = _rocketController.SynchedRotation;
        }
        else
        {
            float lerp = distance >= 0.5f ? 5 : 1;

            transform.position = Vector3.Lerp(transform.position, _rocketController.SynchedPosition, lerp * Time.fixedDeltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, _rocketController.SynchedRotation, lerp * Time.fixedDeltaTime);
        }
    }

    protected override void ControlGravitation()
    {
        
    }

    protected override void ApplyWindForce()
    {

    }
}
