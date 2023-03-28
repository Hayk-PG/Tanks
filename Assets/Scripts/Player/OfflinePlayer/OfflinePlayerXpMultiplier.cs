using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfflinePlayerXpMultiplier: PlayerDropBoxObserver
{
    protected override int Price { get; set; }
    protected override int Quantity { get; set; }

    protected int XpMultiplier { get; set; }

    protected override bool IsAllowed { get; set; }

    protected override TurnState PlayerTurnState { get; set; }




    protected override void OnItemSelect(DropBoxItemType dropBoxItemType, object[] data)
    {
        IsAllowed = dropBoxItemType == DropBoxItemType.Xp2 || dropBoxItemType == DropBoxItemType.Xp3;

        if (IsAllowed)
            Execute(data);
    }

    protected override void Execute(object[] data)
    {
        Price = (int)data[0];
        Quantity = (int)data[1] + GameSceneObjectsReferences.TurnController.TurnCyclesCount;
        XpMultiplier = (int)data[2];

        PlayerTurnState = _playerTankController.OwnTank.gameObject.name == Names.Tank_FirstPlayer ? TurnState.Player1 : TurnState.Player2;

        SetPlayerScoreMultiplier(XpMultiplier);

        ManageTurnControllerSubscription(true);

        _data[0] = Quantity;

        BuffDebuffHandler.RaiseEvent(XpMultiplier <= 2 ? BuffDebuffType.Xp2 : BuffDebuffType.Xp3, PlayerTurnState, _data);
    }

    protected void SetPlayerScoreMultiplier(int xpMultiplier) => _playerTankController._scoreController.SetScoreMultiplier(xpMultiplier);

    protected override void OnTurnController(TurnState turnState)
    {
        if(GameSceneObjectsReferences.TurnController.TurnCyclesCount >= Quantity)
        {
            ManageTurnControllerSubscription(false);

            SetPlayerScoreMultiplier(1);

            return;
        }
    }
}
