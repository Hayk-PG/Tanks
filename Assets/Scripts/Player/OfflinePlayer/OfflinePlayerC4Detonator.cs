using UnityEngine;
using System;


public class OfflinePlayerC4Detonator: PlayerDropBoxObserver
{
    protected Func<TankController, Vector3?> _minePosition;





    protected override void Awake()
    {
        
    }

    protected override void OnItemSelect(DropBoxItemType dropBoxItemType, object[] data)
    {
        if (IsAllowed(dropBoxItemType))
            Execute(data);
    }

    protected override bool IsAllowed(DropBoxItemType dropBoxItemType)
    {
        return dropBoxItemType == DropBoxItemType.C4;
    }

    protected override void Execute(object[] data)
    {
        RetrieveData(data);

        OnC4(_minePosition, _price);
    }

    protected override void RetrieveData(object[] data)
    {
        base.RetrieveData(data);

        AssignMinPosition(data);
    }

    protected virtual void AssignMinPosition(object[] data) => _minePosition = (Func<TankController, Vector3?>)data[2];

    protected virtual void OnC4(Func<TankController, Vector3?> MinePosition, int price)
    {
        TankController tankController = GlobalFunctions.ObjectsOfType<TankController>.Find(tk => tk != _playerTankController.OwnTank);

        Vector3? minePosition = MinePosition(tankController);

        if (minePosition.HasValue)
        {
            DeductScores();

            TriggerMine(minePosition.Value);

            return;
        }

        OnFailedToFindMine();
    }

    protected virtual void TriggerMine(Vector3 minePosition) => GlobalFunctions.ObjectsOfType<Mine>.Find(m => m.transform.position == minePosition).TriggerMine();

    protected virtual void OnFailedToFindMine()
    {

    }
}
