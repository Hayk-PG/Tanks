using UnityEngine;
using Photon.Pun;

public class PhotonNetworkSmokeScreenDetector : MonoBehaviourPun
{
    public void DetectPlayerSmokeScreenImpact(string impactedTankName, bool isImpacted)
    {
        if (MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer))
            photonView.RPC("DetectPlayerSmokeScreenImpactRPC", RpcTarget.AllViaServer, impactedTankName, isImpacted);
    }

    public void DestroySmokeCloud(Vector3 id)
    {
        if (MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer))
            photonView.RPC("DestroySmokeCloudRPC", RpcTarget.AllViaServer, id);
    }

    [PunRPC]
    private void DetectPlayerSmokeScreenImpactRPC(string impactedTankName, bool isImpacted)
    {
        PlayerSmokeScreenDetector playerSmokeScreenDetector = Get<PlayerSmokeScreenDetector>.From(GameObject.Find("impactedTankName"));
       
        if (playerSmokeScreenDetector == null)
            return;

        TankController tankController = Get<TankController>.From(playerSmokeScreenDetector.gameObject);

        if (tankController = null)
            return;

        if (tankController.BasePlayer != null)
            playerSmokeScreenDetector.SetCamerasEnable(!isImpacted);
    }

    [PunRPC]
    private void DestroySmokeCloudRPC(Vector3 id)
    {
        GlobalFunctions.Loop<SmokeCloud>.Foreach(FindObjectsOfType<SmokeCloud>(), smokeCloud =>
        {
            if (smokeCloud.Id == id)
                smokeCloud.DestroySmokeCloud();
        });
    }
}
