using Photon.Realtime;
using UnityEngine;

public class Tab_Lobby : Tab_Base<MyPhotonCallbacks>
{
    [SerializeField] private Transform _transform_Content;
    [SerializeField] private RoomButton _prefab_roomButton;

    private RoomButton _copy_roomButton;


    private void OnEnable()
    {
        _object.OnPhotonJoinedLobby += OnPhotonJoinedLobby;
        _object.OnUpdateRoomList += OnUpdateRoomList;
    }

    private void OnDisable()
    {
        _object.OnPhotonJoinedLobby -= OnPhotonJoinedLobby;
        _object.OnUpdateRoomList -= OnUpdateRoomList;
    }

    private void OnPhotonJoinedLobby()
    {
        base.OpenTab();
    }

    private void OnUpdateRoomList(RoomInfo roomInfo)
    {
        if(_transform_Content.Find(roomInfo.Name) == null)
        {
            _copy_roomButton = Instantiate(_prefab_roomButton, _transform_Content);
            _copy_roomButton.AssignRoomButton(new RoomButton.UI(roomInfo.Name, "---", roomInfo.PlayerCount + "/" + roomInfo.MaxPlayers, null));
        }
        else
        {
            if (roomInfo.RemovedFromList)
            {
                Destroy(_transform_Content.Find(roomInfo.Name).gameObject);
            }
        }        
    }
}
