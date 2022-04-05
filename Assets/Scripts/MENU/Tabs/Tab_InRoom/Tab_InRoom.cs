using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Tab_InRoom : Tab_Base<MyPhotonCallbacks>
{
    [SerializeField] private Text _text_Title;
    [SerializeField] private PlayerInRoom[] _playersInRoom;

    public string RoomName
    {
        get => _text_Title.text;
        private set => _text_Title.text = value;
    }



    private void OnEnable()
    {
        _object.OnRoomJoined += OnRoomJoined;
    }

    private void OnDisable()
    {
        _object.OnRoomJoined += OnRoomJoined;
    }

    private void OnRoomJoined(Room room)
    {
        MenuTabs.Activity(MenuTabs.Tab_InRoom.CanvasGroup);        
        RoomName = room.Name;
        UpdatePlayersInRoom();
    }

    private void HideAllPlayersInRoom()
    {
        foreach (var item in _playersInRoom)
        {
            item.DisplayProperties(new PlayerInRoom.Properties(null, false));
        }
    }

    private void UpdatePlayersInRoom()
    {
        HideAllPlayersInRoom();

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            _playersInRoom[i].DisplayProperties(new PlayerInRoom.Properties(PhotonNetwork.PlayerList[i].NickName, true));
        }
    }
}
