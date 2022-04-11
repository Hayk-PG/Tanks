using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class Network : MonoBehaviour
{
    public static Network Manager { get; set; }
    public PhotonView PhotonView { get; set; }

    public Action<Player> OnInvokeRPCMethode { get; set; }
    public Action<Player> OnLoadLevelRPC { get; set; }


    private void Awake()
    {
        if(Manager != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Manager = this;
            DontDestroyOnLoad(gameObject);
        }

        PhotonView = Get<PhotonView>.From(gameObject);
    }

    public void InvokeRPCMethode(Player localPlayer)
    {
        PhotonView.RPC("RPC", RpcTarget.AllBufferedViaServer, PhotonNetwork.LocalPlayer);
    }

    [PunRPC]
    private void RPC(Player localPlayer)
    {
        OnInvokeRPCMethode?.Invoke(localPlayer);
    }

    public void LoadLevelRPC(Player localPlayer)
    {
        PhotonView.RPC("LoadLevel", RpcTarget.All, PhotonNetwork.LocalPlayer);
    }

    [PunRPC]
    private void LoadLevel(Player localPlayer)
    {
        OnLoadLevelRPC?.Invoke(localPlayer);
    }
}
