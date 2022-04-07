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
        _object.OnRoomPlayerEntered += OnRoomPlayerEntered;
        _object.OnRoomPlayerLeft += OnRoomPlayerEntered;
    }

    private void OnDisable()
    {
        _object.OnRoomJoined += OnRoomJoined;
        _object.OnRoomPlayerEntered -= OnRoomPlayerEntered;
        _object.OnRoomPlayerLeft -= OnRoomPlayerEntered;
    }

    private void OnRoomJoined(Room room)
    {
        base.OpenTab();  
        RoomName = room.Name;
        UpdatePlayersInRoom();
    }

    private void OnRoomPlayerEntered(Player player)
    {
        UpdatePlayersInRoom();
    }

    private void HideAllPlayersInRoom()
    {
        foreach (var item in _playersInRoom)
        {
            item.Hide();
        }
    }

    private void UpdatePlayersInRoom()
    {
        HideAllPlayersInRoom();

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            _playersInRoom[i].AssignPlayerInRoom(new PlayerInRoom.Properties(PhotonNetwork.PlayerList[i].NickName, PhotonNetwork.PlayerList[i].ActorNumber, true));
        }
    }
}
