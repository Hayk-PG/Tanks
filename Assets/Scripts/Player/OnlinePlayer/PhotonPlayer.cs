using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PhotonPlayer : BasePlayer
{
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private string _nickName;
    [SerializeField] private int _actorNumber;

    public PhotonView PhotonView
    {
        get => _photonView;
    }
    public string NickName
    {
        get => _nickName;
        set => _nickName = value;
    }
    public int ActorNumber
    {
        get => _actorNumber;
        set => _actorNumber = value;
    }


    public void InitializePlayer(Player player)
    {
        PhotonView.RPC("Initialize", RpcTarget.AllViaServer, player);
    }

    [PunRPC]
    private void Initialize(Player player)
    {
        NickName = player.NickName;
        ActorNumber = player.ActorNumber;
        AssignGameObjectName(NickName);
    }
}
