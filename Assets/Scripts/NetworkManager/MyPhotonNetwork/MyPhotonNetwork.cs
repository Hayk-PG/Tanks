using Photon.Pun;

public static partial class MyPhotonNetwork 
{
    public static bool IsOfflineMode => PhotonNetwork.OfflineMode;
    public static bool IsConnected = PhotonNetwork.IsConnected;
    public static bool IsConnectedAndReady = PhotonNetwork.IsConnectedAndReady;

    public static void OfflineMode(bool isOfflineMode)
    {
        PhotonNetwork.OfflineMode = isOfflineMode;
    }
}
