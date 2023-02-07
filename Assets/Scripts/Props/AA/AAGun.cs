using UnityEngine;

public class AAGun : MonoBehaviour
{
    [SerializeField] 
    private BaseBulletController _projectile;

    public BaseBulletController Projectile => _projectile;
}
