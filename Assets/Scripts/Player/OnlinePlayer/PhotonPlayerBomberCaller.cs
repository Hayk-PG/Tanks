using UnityEngine;
using Photon.Pun;

public class PhotonPlayerBomberCaller : OfflinePlayerBomberCaller
{
    [SerializeField] [Space]
    private PhotonPlayerController _photonPlayerController;

    protected override bool IsAllowed => _playerTankController?.OwnTank.BasePlayer != null;



    protected override void CallBomber(BomberType bomberType, Vector3 dropPosition)
    {
        // This method calls a bomber for the local player only

        base.CallBomber(bomberType, dropPosition);

        // This method calls a bomber for all other players in the game
        // and should only be called on the server side

        _photonPlayerController.PhotonView.RPC("CallBomberRPC", RpcTarget.Others, bomberType, dropPosition, _playerTankController.OwnTank.name);
    }

    [PunRPC]
    private void CallBomberRPC(BomberType bomberType, Vector3 dropPosition, string ownerName)
    {
        TankController tankController = GlobalFunctions.ObjectsOfType<TankController>.Find(tc => tc.gameObject.name == ownerName);

        IScore iScore = Get<IScore>.From(tankController.gameObject);

        PlayerTurn playerTurn = Get<PlayerTurn>.From(tankController.gameObject);

        GameSceneObjectsReferences.AirSupport.Call(playerTurn, iScore, dropPosition);
    }
}
