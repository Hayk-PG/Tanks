using Photon.Pun;
using Photon.Realtime;

public partial class MyPhotonNetwork 
{
    public static Room CurrentRoom => PhotonNetwork.CurrentRoom;
    public static bool IsInRoom => PhotonNetwork.InRoom;

    public static void LoadLevel()
    {
        PhotonNetwork.LoadLevel(MyScene.Manager.GameSceneName);
    }   
}
