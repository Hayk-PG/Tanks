using Photon.Realtime;
using System;
using UnityEngine;

public class Tab_Lobby : Tab_Base<MyPhotonCallbacks>
{
    [SerializeField] private Transform _transform_Content;
    [SerializeField] private RoomButton _prefab_roomButton;

    private RoomButton _copy_roomButton;


    private void OnEnable()
    {
        //_object._OnJoinedLobby += OpenTab;
        _object.onRoomListUpdate += GetRoomsUpdatedList;
    }

    private void OnDisable()
    {
        //_object._OnJoinedLobby -= OpenTab;
        _object.onRoomListUpdate -= GetRoomsUpdatedList;
    }

    public override void OpenTab()
    {
        if(String.IsNullOrEmpty(MyPhotonNetwork.CurrentLobby.Name))
            base.OpenTab();
    }

    private void GetRoomsUpdatedList(RoomInfo roomInfo)
    {
        if(_transform_Content.Find(roomInfo.Name) == null)
        {
            string roomName = roomInfo.Name;
            string info = "---";
            string playersCount = roomInfo.PlayerCount + "/" + roomInfo.MaxPlayers;
            int mapIndex = roomInfo.CustomProperties.ContainsKey(Keys.MapIndex) ? (int)roomInfo.CustomProperties[Keys.MapIndex] : 0;

            _copy_roomButton = Instantiate(_prefab_roomButton, _transform_Content);
            _copy_roomButton.AssignRoomButton(new RoomButton.UI(roomName, info, playersCount, mapIndex));
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
