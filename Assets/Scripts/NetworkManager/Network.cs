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

    private MyScene _myScene;


    private void Awake()
    {
        Instance();
        _myScene = FindObjectOfType<MyScene>();
        PhotonView = Get<PhotonView>.From(gameObject);
    }

    private void OnEnable()
    {
        _myScene.OnDestroyOnLoadMenuScene += DestroyGameObject;
    }

    private void OnDisable()
    {
        _myScene.OnDestroyOnLoadMenuScene -= DestroyGameObject;
    }

    private void Instance()
    {
        if (Manager != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Manager = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void DestroyGameObject()
    {
        Destroy(gameObject);
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
        PhotonView.RPC("LoadLevel", RpcTarget.AllViaServer, PhotonNetwork.LocalPlayer);
    }

    [PunRPC]
    private void LoadLevel(Player localPlayer)
    {
        OnLoadLevelRPC?.Invoke(localPlayer);
    }
}
