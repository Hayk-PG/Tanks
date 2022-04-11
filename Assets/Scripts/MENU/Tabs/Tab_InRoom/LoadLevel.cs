using Photon.Realtime;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class LoadLevel : MonoBehaviour
{
    private Tab_InRoom _tabInRoom;   
    private MyPlugins _myPlugins;
    private Network _network;
    private MyPhotonCallbacks _myPhotonCallbacks;

    private List<PlayerInRoom> _playerInRoom;


    private void Awake()
    {
        _tabInRoom = FindObjectOfType<Tab_InRoom>();
        _myPlugins = FindObjectOfType<MyPlugins>();
        _network = FindObjectOfType<Network>();
        _myPhotonCallbacks = FindObjectOfType<MyPhotonCallbacks>();

        _playerInRoom = GetComponentsInChildren<PlayerInRoom>().ToList();
    }

    private void OnEnable()
    {
        _network.OnLoadLevelRPC += OnLoadLevelRPC;
        _myPhotonCallbacks._OnLeftRoom += OnLeftRoom;
    }

    private void OnDisable()
    {
        _myPlugins.OnPluginService -= OnPluginService;
        _network.OnLoadLevelRPC -= OnLoadLevelRPC;
        _myPhotonCallbacks._OnLeftRoom -= OnLeftRoom;
    }

    public void Run(Player localPlayer)
    {
        if (localPlayer.IsMasterClient)
        {
            _myPlugins.OnPluginService -= OnPluginService;
            _myPlugins.OnPluginService += OnPluginService;
        }
    }

    private void OnPluginService()
    {
        InvokeLoadLevelRPC(MyPhotonNetwork.LocalPlayer);
    }

    private void InvokeLoadLevelRPC(Player localPlayer)
    {
        _network.LoadLevelRPC(localPlayer);
    }

    private void OnLoadLevelRPC(Player player)
    {
        foreach (var p in MyPhotonNetwork.PlayersList)
        {
            print(p.NickName + "/" + _tabInRoom.IsPlayerReady(p) + "/" + _playerInRoom.Find(number => number.PlayerActorNumber == p.ActorNumber)?.IsPlayerReady);
        }
    }

    private void OnLeftRoom()
    {
        _myPlugins.OnPluginService -= OnPluginService;
    }
}
