using Photon.Realtime;
using UnityEngine;

public class LoadLevel : MonoBehaviour
{
    private Tab_Room _tabRoom;   
    private MyPlugins _myPlugins;
    private Network _network;
    private MyPhotonCallbacks _myPhotonCallbacks;


    private void Awake()
    {
        _tabRoom = FindObjectOfType<Tab_Room>();
        _myPlugins = FindObjectOfType<MyPlugins>();
        _network = FindObjectOfType<Network>();
        _myPhotonCallbacks = FindObjectOfType<MyPhotonCallbacks>();
    }

    private void OnEnable()
    {
        _network.OnLoadLevelRPC += OnLoadLevelRPC;
        _myPhotonCallbacks._OnLeftRoom += OnLeftRoom;
    }

    private void OnDisable()
    {
        UnsuscribeFromPluginService();
        _network.OnLoadLevelRPC -= OnLoadLevelRPC;
        _myPhotonCallbacks._OnLeftRoom -= OnLeftRoom;
    }

    private void SubscribeToPluginService()
    {
        _myPlugins.OnPluginService += OnPluginService;
    }

    private void UnsuscribeFromPluginService()
    {
        _myPlugins.OnPluginService -= OnPluginService;
    }

    public void Run(Player localPlayer)
    {
        if (localPlayer.IsMasterClient)
        {
            UnsuscribeFromPluginService();
            SubscribeToPluginService();
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
        Conditions<bool>.Compare(player.IsMasterClient, MasterClientLoadLevel, null);
    }

    private void MasterClientLoadLevel()
    {
        int readyPlayersCount = 0;

        foreach (var p in MyPhotonNetwork.PlayersList)
        {
            if (_tabRoom.IsPlayerReady(p)) readyPlayersCount++;
        }

        if (readyPlayersCount == MyPhotonNetwork.PlayersList.Length)
        {
            UnsuscribeFromPluginService();
            MyPhotonNetwork.LoadLevel();
        }
    }

    private void OnLeftRoom()
    {
        UnsuscribeFromPluginService();
    }
}
