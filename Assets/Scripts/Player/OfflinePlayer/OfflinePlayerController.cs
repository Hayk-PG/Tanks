
public class OfflinePlayerController : BasePlayer
{
    public void Initialize()
    {
        AssignGameObjectName("Player");
        Get<OfflinePlayerTankSpawner>.From(gameObject).SpawnTanks(Data.Manager.SelectedTankIndex, 0);
        Get<OfflinePlayerTankSpawner>.From(gameObject).SpawnAiTank(Data.Manager.SelectedAITankIndex);
        PlayerReady(0);
    }
}
