using UnityEngine;
using Photon.Pun;

public class PhotonNetworkBoostZoneManager : MonoBehaviourPun
{
    public void OnEnter(Vector3 id, TankController tankController)
    {
        if (MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer))
            photonView.RPC("OnEnterRPC", RpcTarget.AllViaServer, id, tankController.name);
    }

    [PunRPC]
    private void OnEnterRPC(Vector3 id, string name)
    {
        BoostZoneManager boostZoneManager = GlobalFunctions.ObjectsOfType<BoostZoneManager>.Find(boostZone => boostZone.ID == id);
        TankController tankController = GlobalFunctions.ObjectsOfType<TankController>.Find(tc => tc.name == name);

        if (boostZoneManager != null && tankController != null)
            boostZoneManager.OnEnter(tankController);
    }

    public void OnExit(Vector3 id, TankController tankController)
    {
        if (MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer))
            photonView.RPC("OnExitRPC", RpcTarget.AllViaServer, id, tankController.name);
    }

    [PunRPC]
    private void OnExitRPC(Vector3 id, string name)
    {
        BoostZoneManager boostZoneManager = GlobalFunctions.ObjectsOfType<BoostZoneManager>.Find(boostZone => boostZone.ID == id);
        TankController tankController = GlobalFunctions.ObjectsOfType<TankController>.Find(tc => tc.name == name);

        if (boostZoneManager != null && tankController != null)
            boostZoneManager.OnExit(tankController);
    }
}
