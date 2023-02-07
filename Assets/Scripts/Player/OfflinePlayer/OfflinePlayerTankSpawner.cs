
public class OfflinePlayerTankSpawner : BasePlayerTankSpawner<OfflinePlayerTankController>
{
    protected override void CacheSpawnedTank(TankController tank)
    {
        _tankController.CacheTank(tank);
    }

    public void SpawnAiTank(int tankIndex)
    {
        TankController aiTank = Instantiate(Data.Manager.AvailableAITanks[tankIndex]._tank, _spawnPointForPlayer2.position, _spawnPointForPlayer2.rotation);
        Get<PlayerTurn>.From(aiTank.gameObject).MyTurn = TurnState.Player2;

        aiTank.gameObject.name = Names.Tank_SecondPlayer;

        _gameManager.Tank2 = aiTank;
    }
}
