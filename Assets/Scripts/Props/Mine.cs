using UnityEngine;
using UnityEngine.AddressableAssets;


//ADDRESSABLE
public class Mine : MetalTile
{
    [SerializeField]
    private Explosion _explosion;

    private bool _isTriggered;

    public Vector3 ID { get; private set; }



    protected override void Start()
    {
        base.Start();

        ID = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Get<TankController>.From(other.gameObject) == null)
            return;

        if (!_isTriggered)
        {
            if (MyPhotonNetwork.IsOfflineMode)
                TriggerMine();
            else
                TriggerMineInOnlineMode();

            _isTriggered = true;
        }
    }

    protected override void PlaySoundFX()
    {
        
    }

    protected override void OnDestruction() => Addressables.Release(_meshObj);

    public void TriggerMine()
    {
        _explosion.gameObject.SetActive(true);
        _explosion.transform.SetParent(null);

        _tile.Destruct(100, 0);
    }

    private void TriggerMineInOnlineMode()
    {
        GameSceneObjectsReferences.PhotonNetworkMineController.TriggerMine(ID);
    }
}
