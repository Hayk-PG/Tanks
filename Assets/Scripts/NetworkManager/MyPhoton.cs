using UnityEngine;
using Photon.Pun;


public partial class MyPhoton : MonoBehaviour
{
    public static void StartConnection()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
}
