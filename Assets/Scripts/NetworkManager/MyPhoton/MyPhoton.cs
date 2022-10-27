using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public partial class MyPhoton : MonoBehaviour
{
    public static void Connect(MyPhotonAuthValues myPhotonAuthValues)
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.NickName = myPhotonAuthValues.Nickname;
        PhotonNetwork.AuthValues = new AuthenticationValues { UserId = myPhotonAuthValues.UserID };
    }

    public static void Disconnect()
    {
        if (PhotonNetwork.IsConnected) PhotonNetwork.Disconnect();
    }
}
