using Photon.Pun;

public static partial class MyPhotonNetwork 
{
    public static void OfflineMode(bool isOfflineMode)
    {
        PhotonNetwork.OfflineMode = isOfflineMode;
    }

    public static bool IsOfflineMode()
    {
        return PhotonNetwork.OfflineMode;
    }
}
