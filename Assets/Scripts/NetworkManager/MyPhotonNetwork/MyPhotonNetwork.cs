using Photon.Pun;

public static partial class MyPhotonNetwork 
{
    public static bool IsOfflineMode => PhotonNetwork.OfflineMode;

    public static void OfflineMode(bool isOfflineMode)
    {
        PhotonNetwork.OfflineMode = isOfflineMode;
    }
}
