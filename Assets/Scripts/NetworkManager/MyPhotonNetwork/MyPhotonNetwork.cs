using Photon.Pun;

public static partial class MyPhotonNetwork 
{
    public static bool IsOfflineMode => PhotonNetwork.OfflineMode;
    public static bool IsConnected => PhotonNetwork.IsConnected;
    public static bool IsConnectedAndReady => PhotonNetwork.IsConnectedAndReady;

    public static void ManageOfflineMode(bool isOfflineMode)
    {
        if (IsOfflineMode == isOfflineMode)
            return;

        PhotonNetwork.OfflineMode = isOfflineMode;
    }
}
