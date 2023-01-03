using UnityEngine;

public class AAProjectileArsenal : MonoBehaviour
{
    [SerializeField] private BulletController _missile;

    public BulletController Missile => _missile;
}
