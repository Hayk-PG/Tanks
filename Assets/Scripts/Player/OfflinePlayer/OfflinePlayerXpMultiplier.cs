using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfflinePlayerXpMultiplier: PlayerDropBoxObserver
{
    protected override void OnItemSelect(DropBoxSelectionHandler.DropBoxItemType dropBoxItemType, object[] data)
    {
        _isAllowed = dropBoxItemType == DropBoxSelectionHandler.DropBoxItemType.XpMultiplier;

        if (_isAllowed)
            return;

        int multiplier = (int)data[0];

        _activeTurnsCount = (int)data[1] + GameSceneObjectsReferences.TurnController.TurnCyclesCount;

        _playerTankController._scoreController.SetScoreMultiplier(multiplier);
    }

    protected override void OnTurnController(TurnState turnState)
    {
        if(_activeTurnsCount >= GameSceneObjectsReferences.TurnController.TurnCyclesCount)
        {
            return;
        }
    }
}
