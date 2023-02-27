using UnityEngine;

public class OfflinePlayerShieldController : MonoBehaviour
{
    [SerializeField]
    protected BasePlayerTankController<BasePlayer> _playerTankController;



    protected virtual void OnEnable() => GameSceneObjectsReferences.DropBoxSelectionPanelShield.onShield += OnActivateShield;

    protected virtual void OnDisable() => GameSceneObjectsReferences.DropBoxSelectionPanelShield.onShield -= OnActivateShield;

    protected virtual void OnActivateShield(int price)
    {
        _playerTankController._scoreController.GetScore(price, null);

        _playerTankController._playerShields.ActivateShields(ShieldIndex());
    }

    protected virtual int ShieldIndex()
    {
        return _playerTankController._playerTurn.MyTurn == TurnState.Player1 ? 0 : 1;
    }
}
