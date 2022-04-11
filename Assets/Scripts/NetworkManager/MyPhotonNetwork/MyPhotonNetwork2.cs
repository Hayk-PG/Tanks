using Photon.Pun;
using Photon.Realtime;

public partial class MyPhotonNetwork 
{
    public static Room CurrentRoom => PhotonNetwork.CurrentRoom;

    public static void LoadLevel()
    {
        PhotonNetwork.LoadLevel(MyScene.Manager.GameSceneName);
    }   
}
