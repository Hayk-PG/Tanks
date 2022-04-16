using UnityEngine;

public class OfflinePlayerController : BasePlayer
{
    public void Initialize()
    {
        AssignGameObjectName("Player");
        Get<OfflinePlayerTankSpawner>.From(gameObject).SpawnTanks(0, 0);
        PlayerReady(0);
    }
}
