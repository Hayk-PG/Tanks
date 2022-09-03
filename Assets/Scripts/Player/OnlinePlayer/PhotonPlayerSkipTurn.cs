using Photon.Pun;

public class PhotonPlayerSkipTurn : PhotonPlayerBaseRPC, ISkipTurn
{
    public void SkipTurn(TurnState turnState)
    {
        _photonPlayerController.PhotonView.RPC("SkipTurnRPC", RpcTarget.AllViaServer, turnState);
    }

    [PunRPC]
    private void SkipTurnRPC(TurnState turnState)
    {
        _photonPlayerTankController?._playerTankSkipTurn.Skip(turnState);
    }
}
