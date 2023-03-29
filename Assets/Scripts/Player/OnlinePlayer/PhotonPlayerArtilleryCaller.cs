using UnityEngine;
using Photon.Pun;

public class PhotonPlayerArtilleryCaller : OfflinePlayerArtilleryCaller
{
    [SerializeField] [Space]
    private PhotonPlayerController _photonPlayerController;



    protected override bool IsAllowed(BaseRemoteControlTarget.Mode mode)
    {
        return mode == BaseRemoteControlTarget.Mode.Artillery && _playerTankController?.OwnTank.BasePlayer != null;
    }

    protected override void CallArtillery(object[] targetData)
    {
        string ownerName = _playerTankController.OwnTank.name;

        int shellsCount = (int)targetData[2];

        float shellsSpreadValue = (float)targetData[0];

        Vector3 target = (Vector3)targetData[3];

        _photonPlayerController.PhotonView.RPC("CallArtilleryRPC", RpcTarget.AllViaServer, ownerName, shellsCount, shellsSpreadValue, target);
    }

    [PunRPC]
    private void CallArtilleryRPC(string ownerName, int shellsCount, float shellSpreadValue, Vector3 target)
    {
        object[] data = new object[]
        {
            Get<PlayerTurn>.From(GameObject.Find("ownerName")),
            Get<IScore>.From(GameObject.Find("ownerName")),
            RandomShellSpreadValues(shellsCount, shellSpreadValue),
            target
        };

        GameSceneObjectsReferences.ArtillerySupport.Call(data);
    }
}