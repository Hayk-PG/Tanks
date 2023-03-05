using System.Collections;
using UnityEngine;

public class MultipleShootController : ShootController
{
    private int _shootPointsCount = 8;

    protected override void InstantiateBullet(float force)
    {  
        StartCoroutine(InstantiateBulletCoroutine(force));
    }

    private IEnumerator InstantiateBulletCoroutine(float force)
    {
        int lastIndex = _shootPointsCount - 1;

        for (int i = 0; i < _shootPointsCount; i++)
        {
            float randomForce = i < lastIndex ? Random.Range(force - 0.5f, force + 0.5f) : force;
            Bullet = Instantiate(_playerAmmoType._weapons[ActiveAmmoIndex]._prefab, _shootPoint.position, _canonPivotPoint.rotation);
            Bullet.OwnerScore = _iScore;
            Bullet.RigidBody.velocity = Bullet.transform.forward * randomForce;
            _gameManagerBulletSerializer.BaseBulletController = Bullet;
            
            if (i == lastIndex)
            {
                GameSceneObjectsReferences.MainCameraController.CameraOffset(_playerTurn, Bullet.RigidBody, null, null);
                Bullet.IsLastShellOfBarrage = true;
            }
            yield return new WaitForSeconds(0.4f);
        }
    }

    protected override bool HaveEnoughBullets()
    {
        return _playerAmmoType._weaponsBulletsCount[ActiveAmmoIndex] > _shootPointsCount;
    }
}
