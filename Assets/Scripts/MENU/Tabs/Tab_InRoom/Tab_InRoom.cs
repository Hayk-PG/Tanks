using UnityEngine;
using UnityEngine.UI;
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
        _object._OnJoinedRoom += OnJoinedRoom;
        _object._OnPlayerEnteredRoom += OnPlayerEnteredRoom;
        _object._OnPlayerLeftRoom += OnPlayerLeftRoom;
        _object._OnLeftRoom += OnLeftRoom;
    }

    private void OnDisable()
    {
        _object._OnJoinedRoom += OnJoinedRoom;
        _object._OnPlayerEnteredRoom -= OnPlayerEnteredRoom;
        _object._OnPlayerLeftRoom -= OnPlayerLeftRoom;
        _object._OnLeftRoom -= OnLeftRoom;
    }

    private void OnJoinedRoom(Room room)
    {
        RoomName = room.Name;
        UpdatePlayersInRoom();
        base.OpenTab();              
    }

    private void OnPlayerEnteredRoom(Player player)
    {
        UpdatePlayersInRoom();
    }

    private void OnPlayerLeftRoom(Player player)
    {
        UpdatePlayersInRoom();
    }

    private void OnLeftRoom()
    {
        ResetPlayersInRoom();
    }

    private void UpdatePlayersInRoom()
    {
        ResetPlayersInRoom();
        AssignPlayersInRoom();
    }

    private void ResetPlayersInRoom()
    {
        foreach (var playerInRoom in _playersInRoom)
        {
            playerInRoom.ResetPlayerInRoom();
        }
    }

    private void AssignPlayersInRoom()
    {
        for (int i = 0; i < MyPhotonNetwork.PlayersList.Length; i++)
        {
            _playersInRoom[i].AssignPlayerInRoom(new PlayerInRoom.Properties
                (
                MyPhotonNetwork.PlayersList[i].NickName,
                MyPhotonNetwork.PlayersList[i].ActorNumber,
                IsPlayerReady(MyPhotonNetwork.PlayersList[i])
                ));
        }
    }

    private bool IsPlayerReady(Player player)
    {
        return !player.CustomProperties.ContainsKey(PlayerCustomPropertiesKeys.IsPlayerReady) ? false :
                (bool)player.CustomProperties[PlayerCustomPropertiesKeys.IsPlayerReady];
    }
}
