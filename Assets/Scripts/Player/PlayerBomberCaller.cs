using UnityEngine;

public class PlayerBomberCaller : MonoBehaviour
{
    [SerializeField]
    protected BasePlayerTankController<BasePlayer> _playerTankController;



    protected virtual void OnEnable()
    {
        GlobalFunctions.Loop<DropBoxSelectionPanelBomber>.Foreach(GameSceneObjectsReferences.DropBoxSelectionPanelBombers, bombers => { bombers.onCallBomber += CallBomber; });
    }

    protected virtual void OnDisable()
    {
        GlobalFunctions.Loop<DropBoxSelectionPanelBomber>.Foreach(GameSceneObjectsReferences.DropBoxSelectionPanelBombers, bombers => { bombers.onCallBomber -= CallBomber; });
    }

    protected virtual void CallBomber(BomberType bomberType, int price, int quantity)
    {
        if (_playerTankController.OwnTank == null)
            return;


        //GameSceneObjectsReferences.AirSupport.Call(_playerTankController._playerTurn, _playerTankController._iScore, )
    }
}
