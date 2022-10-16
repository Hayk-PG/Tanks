public class GiveTurnContent : IWoodBoxContent
{
    private TurnController _turnController;


    public GiveTurnContent(TurnController turnController)
    {
        _turnController = turnController;
    }

    public void Use(TankController tankController, Tab_WoodboxContent tab_WoodboxContent)
    {
        PlayerTurn playerTurn = Get<PlayerTurn>.From(tankController.gameObject);

        if (playerTurn != null)
        {
            if (playerTurn.IsMyTurn)
            {
                _turnController.SetNextTurn(playerTurn.MyTurn == TurnState.Player1 ? TurnState.Player2 : TurnState.Player1);
            }
        }
    }
}
