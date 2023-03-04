using UnityEngine;

public class GuidedMissileVelocity : BaseBulletVelocity
{
    [SerializeField] [Space]
    protected float _movementSpeed, _rotationSpeed;

    protected float _rotationX;
    protected float _rotationY;   

    protected Quaternion _rotation;

    protected float Vertical
    {
        get => GameSceneObjectsReferences.Tab_GuidedMissileController.Direction.y;
    }
    protected float Horizontal
    {
        get => GameSceneObjectsReferences.Tab_GuidedMissileController.Direction.x == 0 ? 0 : _rotationY > 0 ? GameSceneObjectsReferences.Tab_GuidedMissileController.Direction.x :
               -GameSceneObjectsReferences.Tab_GuidedMissileController.Direction.x;
    }




    private void Awake() => _rotationY = Converter.AngleConverter(_baseBulletController.RigidBody.transform.eulerAngles.y);

    protected override void ControlLookRotation()
    {
        _rotationX = Mathf.Atan2(Vertical, Horizontal) * Mathf.Rad2Deg;

        _rotation = Quaternion.Euler(-_rotationX, _rotationY, 0);

        _baseBulletController.RigidBody.rotation = Quaternion.Lerp(_baseBulletController.RigidBody.rotation, _rotation, _rotationSpeed * Time.fixedDeltaTime);
    }

    protected override void ControlMovement()
    {
        _baseBulletController.RigidBody.position += _baseBulletController.RigidBody.transform.forward * (_movementSpeed + Horizontal) * Time.fixedDeltaTime;
    }

    protected override void ControlGravitation()
    {
        
    }

    protected override void ApplyWindForce()
    {

    }
}
