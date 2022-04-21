using UnityEngine;

public class GameManagerBulletSerializer : MonoBehaviour
{
    [SerializeField] BulletController _bulletController;

    public BulletController BulletController
    {
        get => _bulletController;
        set => _bulletController = value;
    }
}
