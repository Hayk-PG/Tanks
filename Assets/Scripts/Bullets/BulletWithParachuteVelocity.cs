using UnityEngine;

public class BulletWithParachuteVelocity : BulletVelocity
{
    private delegate void Velocity(BulletController.VelocityData velocityData);
    private Velocity _onLookRotAndWindVelocity;
    private Velocity _onDesceding;

    [SerializeField]
    private GameObject _parachute;


    private void Start()
    {
        _onLookRotAndWindVelocity += BulletLookRotation;
        _onLookRotAndWindVelocity += ApplyWindForceToTheMovement;
        _onDesceding = OnDesceding;
    }

    protected override void OnBulletVelocity(BulletController.VelocityData velocityData)
    {
        Conditions<bool>.Compare(velocityData._rigidBody.velocity.y > 0, () => _onLookRotAndWindVelocity(velocityData), ()=> _onDesceding(velocityData));
    }

    private void OnDesceding(BulletController.VelocityData velocityData)
    {
        OpenParachute();
        _onLookRotAndWindVelocity(velocityData);

        velocityData._rigidBody.velocity = Vector3.down * 30 * Time.fixedDeltaTime;
    }

    private void OpenParachute()
    {
        if (!_parachute.activeInHierarchy)
        {
            _parachute.SetActive(true);
            _bulletController._particles._trail.Stop();
        }
    }
}
