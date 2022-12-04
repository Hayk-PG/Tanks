using UnityEngine;
using Photon.Realtime;
using TMPro;

public class Tab_Room : Tab_Base
{
    [SerializeField] private PlayerInRoom[] _playersInRoom;
    [SerializeField] private TMP_Text _txtRoomName;
    private MyPhotonCallbacks _myPhotonCallbacks;

    public string RoomName
    {
        get => _txtRoomName.text;
        private set => _txtRoomName.text = value;
    }

    protected override void Awake()
    {
        base.Awake();
        _myPhotonCallbacks = FindObjectOfType<MyPhotonCallbacks>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _myPhotonCallbacks._OnJoinedRoom += OnJoinedRoom;
        _myPhotonCallbacks._OnPlayerEnteredRoom += OnPlayerEnteredRoom;
        _myPhotonCallbacks._OnPlayerLeftRoom += OnPlayerLeftRoom;
        _myPhotonCallbacks._OnLeftRoom += OnLeftRoom;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _myPhotonCallbacks._OnJoinedRoom += OnJoinedRoom;
        _myPhotonCallbacks._OnPlayerEnteredRoom -= OnPlayerEnteredRoom;
        _myPhotonCallbacks._OnPlayerLeftRoom -= OnPlayerLeftRoom;
        _myPhotonCallbacks._OnLeftRoom -= OnLeftRoom;
    }

    protected override void GoBack()
    {
        MyPhoton.LeaveRoom();
    }

    private void OnJoinedRoom(Room room)
    {
        RoomName = (string)room.CustomProperties[Keys.RoomFakeName];

        string[][] itemsName = (string[][])room.CustomProperties[Keys.ItemName];
        int[][] itemsAmount = (int[][])room.CustomProperties[Keys.ItemAmount];

        for (int i = 0, j = 0; i < itemsName[0].Length; i++, j ++)
        {
            print(itemsName[0][i] + "/" + itemsAmount[0][i]);
        }
        
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
            PlayerInRoom.Properties properties = new PlayerInRoom.Properties
            {
                _playerName = MyPhotonNetwork.PlayersList[i].NickName,
                _actorNumber = MyPhotonNetwork.PlayersList[i].ActorNumber,
                _playerLevel = 1,
                _isPlayerReady = IsPlayerReady(MyPhotonNetwork.PlayersList[i])
            };

            _playersInRoom[i].AssignPlayerInRoom(properties);
        }
    }

    public bool IsPlayerReady(Player player)
    {
        return !player.CustomProperties.ContainsKey(Keys.IsPlayerReady) ? false :
                (bool)player.CustomProperties[Keys.IsPlayerReady];
    }
}
