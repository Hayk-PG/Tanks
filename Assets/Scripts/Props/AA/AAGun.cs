using UnityEngine;

public class AAGun : MonoBehaviour
{
    [SerializeField] private BulletController _projectile;

    public BulletController Projectile => _projectile;
}
