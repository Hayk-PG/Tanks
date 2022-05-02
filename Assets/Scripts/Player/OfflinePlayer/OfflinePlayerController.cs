
public class OfflinePlayerController : BasePlayer
{
    public void Initialize()
    {
        AssignGameObjectName("Player");
        Get<OfflinePlayerTankSpawner>.From(gameObject).SpawnTanks(Data.Manager.SelectedTankIndex, 0);
        PlayerReady(0);
    }
}
