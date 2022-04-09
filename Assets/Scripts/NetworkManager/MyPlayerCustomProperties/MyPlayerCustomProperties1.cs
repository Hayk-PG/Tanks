using Photon.Realtime;
using UnityEngine;

public partial class MyPlayerCustomProperties : MonoBehaviour
{
    private MyPhotonCallbacks _myPhotonCallbacks;

    private void Awake()
    {
        _myPhotonCallbacks = Get<MyPhotonCallbacks>.From(gameObject);
    }

    private void OnEnable()
    {
        _myPhotonCallbacks._OnConnectedToMaster += OnConnectedToMaster;
        _myPhotonCallbacks._OnJoinedRoom += OnJoinedRoom;
        _myPhotonCallbacks._OnLeftRoom += OnLeftRoom;
    }

    private void OnDisable()
    {
        _myPhotonCallbacks._OnConnectedToMaster -= OnConnectedToMaster;
        _myPhotonCallbacks._OnJoinedRoom -= OnJoinedRoom;
        _myPhotonCallbacks._OnLeftRoom -= OnLeftRoom;       
    }

    private void OnConnectedToMaster()
    {
        
    }

    private void OnJoinedRoom(Room room)
    {
        
    }

    private void OnLeftRoom()
    {
        UpdatePlayerCustomProperties(MyPhotonNetwork.LocalPlayer, PlayerCustomPropertiesKeys.IsPlayerReady, false);
    }
}
