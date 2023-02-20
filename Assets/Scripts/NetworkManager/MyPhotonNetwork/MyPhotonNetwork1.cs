using Photon.Pun;
using Photon.Realtime;

public static partial class MyPhotonNetwork
{
    public static Player[] PlayersList => PhotonNetwork.PlayerList;
    public static Player LocalPlayer => PhotonNetwork.LocalPlayer;
    public static Player MasterClient => PhotonNetwork.MasterClient;

    public static bool IsMasterClient(Player player)
    {
        return player.IsMasterClient;
    }

    public static bool AmPhotonViewOwner(PhotonView photonView)
    {
        return photonView.IsMine && photonView.AmOwner;
    }

    public static void SendAllOutgoingCommands()
    {
        PhotonNetwork.SendAllOutgoingCommands();
    }
}
