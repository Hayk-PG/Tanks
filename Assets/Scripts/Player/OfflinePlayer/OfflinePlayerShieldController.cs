using UnityEngine;

public class OfflinePlayerShieldController : MonoBehaviour
{
    [SerializeField]
    protected BasePlayerTankController<BasePlayer> _playerTankController;



    protected virtual void OnEnable()
    {
        DropBoxSelectionHandler.onItemSelect += delegate (DropBoxItemType dropBoxItemType, object[] data)
        {
            if (dropBoxItemType != DropBoxItemType.Shield)
                return;

            OnActivateShield((int)data[0]);
        };
    }

    protected virtual void OnActivateShield(int price)
    {
        _playerTankController._scoreController.GetScore(price, null, null, Vector3.zero);

        _playerTankController._playerShields.ActivateShields(ShieldIndex());
    }

    protected virtual int ShieldIndex()
    {
        return _playerTankController._playerTurn.MyTurn == TurnState.Player1 ? 0 : 1;
    }
}
