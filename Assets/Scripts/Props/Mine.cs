using UnityEngine;
using UnityEngine.AddressableAssets;
using System;


//ADDRESSABLE
public class Mine : MetalTile
{
    [SerializeField]
    private Explosion _explosion;

    private bool _isTriggered;

    private Action onTriggerEnter;

    public Vector3 ID { get; private set; }



    protected override void Start()
    {
        base.Start();

        ID = transform.position;

        onTriggerEnter = MyPhotonNetwork.IsOfflineMode ? TriggerMine : TriggerMineInOnlineMode;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_isTriggered)
        {
            onTriggerEnter();

            _isTriggered = true;
        }
    }

    protected override void PlaySoundFX()
    {
        
    }

    protected override void OnDestruction() => Addressables.Release(_meshObj);

    private void TriggerMineInOnlineMode()
    {
        GameSceneObjectsReferences.PhotonNetworkMineController.TriggerMine(ID);
    }

    public void TriggerMine()
    {
        _explosion.gameObject.SetActive(true);
        _explosion.transform.SetParent(null);

        _tile.Destruct(100, 0);
    }
}
