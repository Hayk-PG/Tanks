public class OfflinePlayerSkipTurn : OfflinePlayerBase, ISkipTurn
{
    public void SkipTurn(TurnState turnState)
    {
        _offlinePlayerTankController?._playerTankSkipTurn.Skip(turnState);
    }
}
