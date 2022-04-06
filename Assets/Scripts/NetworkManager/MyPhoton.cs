using UnityEngine;
using Photon.Pun;


public partial class MyPhoton : MonoBehaviour
{
    public static void StartConnection()
    {
        if (!PhotonNetwork.IsConnected) PhotonNetwork.ConnectUsingSettings();
    }

    public static void Disconnect()
    {
        if (PhotonNetwork.IsConnected) PhotonNetwork.Disconnect();
    }
}
