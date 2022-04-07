using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class Network : MonoBehaviour
{
    public static Network Manager { get; set; }
    public PhotonView PhotonView { get; set; }
    public Action<Player> OnInvokeRPCMethode { get; set; }


    private void Awake()
    {
        if(Manager != null)
        {
            Destroy(Manager);
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
        PhotonView.RPC("RPC", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer);
    }

    [PunRPC]
    private void RPC(Player localPlayer)
    {
        OnInvokeRPCMethode?.Invoke(localPlayer);
    }
}
