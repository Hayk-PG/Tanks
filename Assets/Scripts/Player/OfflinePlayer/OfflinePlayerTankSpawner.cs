
public class OfflinePlayerTankSpawner : BasePlayerTankSpawner<OfflinePlayerTankController>
{
    protected override void CacheSpawnedTank(TankController tank)
    {
        _tankController.CacheTank(tank);
    }

    public void SpawnAiTank(int tankIndex)
    {
        TankController aiTank = Instantiate(Data.Manager.AvailableAITanks[tankIndex]._tank, _spawnPointForPlayer2.position, _spawnPointForPlayer2.rotation);
        if (aiTank.GetComponent<PlayerTurn>() != null)
            aiTank.GetComponent<PlayerTurn>().MyTurn = TurnState.Player2;
        _gameManager.Tank2 = aiTank;
    }
}
