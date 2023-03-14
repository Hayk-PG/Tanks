using UnityEngine;
using System;


public class OfflinePlayerC4Detonator: MonoBehaviour
{
    [SerializeField]
    protected BasePlayerTankController<BasePlayer> _playerTankController;

    protected virtual bool IsAllowed { get; } = true;



    protected virtual void OnEnable()
    {
        GameSceneObjectsReferences.DropBoxSelectionPanelC4.onC4 += OnC4;
    }

    protected virtual void OnDisable()
    {
        GameSceneObjectsReferences.DropBoxSelectionPanelC4.onC4 -= OnC4;
    }

    protected virtual void OnC4(Func<TankController, Vector3> MinePosition, int price)
    {
        if (!IsAllowed)
            return;

        TriggerMine(MinePosition, price);
    }

    protected virtual void TriggerMine(Func<TankController, Vector3> MinePosition, int price)
    {
        TankController tankController = GlobalFunctions.ObjectsOfType<TankController>.Find(tk => tk != _playerTankController.OwnTank);

        Vector3 minePosition = MinePosition(tankController);

        if(minePosition == Vector3.zero)
        {
            OnFailedToFindMine();

            return;
        }

        DeductScores(price);

        TriggerMine(minePosition);
    }

    protected virtual void TriggerMine(Vector3 minePosition)
    {
        GlobalFunctions.ObjectsOfType<Mine>.Find(m => m.transform.position == minePosition).TriggerMine();
    }

    protected virtual void DeductScores(int price)
    {
        _playerTankController._scoreController.GetScore(price, null);
    }

    protected virtual void OnFailedToFindMine()
    {

    }
}
