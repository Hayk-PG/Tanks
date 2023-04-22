using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PhotonPlayerController : BasePlayer
{
    [SerializeField] 
    private PhotonView _photonView;

    [SerializeField] [Space]
    private string _nickName;

    [SerializeField] [Space]
    private int _actorNumber;

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
        PhotonView.RPC("Initialize", RpcTarget.AllViaServer, player, Data.Manager.SelectedTankIndex);
    }

    [PunRPC]
    private void Initialize(Player player, int spawnTankIndex)
    {
        NickName = player.NickName;
        ActorNumber = player.ActorNumber;

        AssignGameObjectName(NickName);

        Get<PhotonPlayerTankSpawner>.From(gameObject).SpawnTanks(spawnTankIndex, player.IsMasterClient ? 0 : 1);

        PlayerReady(player.IsMasterClient ? 0 : 1);
    }
}
