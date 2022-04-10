using Photon.Pun;
using Photon.Realtime;

public partial class MyPhotonNetwork 
{
    public static void LoadLevel()
    {
        PhotonNetwork.LoadLevel(MyScene.Manager.GameSceneName);
    }
}
