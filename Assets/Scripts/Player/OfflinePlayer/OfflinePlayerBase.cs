using UnityEngine;

public class OfflinePlayerBase : MonoBehaviour
{
    protected OfflinePlayerController _offlinePlayerController;
    protected OfflinePlayerTankController _offlinePlayerTankController;


    protected virtual void Awake()
    {
        _offlinePlayerController = Get<OfflinePlayerController>.From(gameObject);
        _offlinePlayerTankController = Get<OfflinePlayerTankController>.From(gameObject);
    }
}
