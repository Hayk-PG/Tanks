using UnityEngine;


public class LobbyCell : MonoBehaviour
{
    [SerializeField] private LobbyItem[] _lobbyItemsEntryFee;
    [SerializeField] private LobbyItem[] _lobbyItemsWin;
    [SerializeField] private LobbyItem[] _lobbyItemsLose;
    [SerializeField] private LobbyGUI _lobbyGUI;
    [SerializeField] private Btn _btnJoinLobby;
    private MyPhotonCallbacks _myPhotonCallbacks;


    private void Awake()
    {
        _myPhotonCallbacks = FindObjectOfType<MyPhotonCallbacks>();
    }

    private void OnEnable()
    {
        _btnJoinLobby.onSelect += JoinLobby;
        _myPhotonCallbacks._OnJoinedLobby += TriggerJoinedLobby;
        _myPhotonCallbacks._OnJoinedRoom += TriggerJoinedRoom;
    }

    private void OnDisable()
    {
        _btnJoinLobby.onSelect -= JoinLobby;
        _myPhotonCallbacks._OnJoinedLobby -= TriggerJoinedLobby;
        _myPhotonCallbacks._OnJoinedRoom -= TriggerJoinedRoom;
    }

    private void JoinLobby()
    {
        MyPhoton.JoinLobby(_lobbyGUI.LobbyName, Photon.Realtime.LobbyType.AsyncRandomLobby);
    }

    private void JoinRoom()
    {
        if (MyPhotonNetwork.CurrentLobby.Name == _lobbyGUI.LobbyName)
        {
            MyPhoton.JoinRandomOrCreateRoomParameters joinRandomOrCreateRoomParameters = new MyPhoton.JoinRandomOrCreateRoomParameters
            {
                TypedLobby = new Photon.Realtime.TypedLobby(_lobbyGUI.LobbyName, Photon.Realtime.LobbyType.AsyncRandomLobby),
                ExpectedMaxPlayers = 2,
                MatchmakingMode = Photon.Realtime.MatchmakingMode.FillRoom,
                RoomOptions = new Photon.Realtime.RoomOptions
                {
                    CleanupCacheOnLeave = true,
                    MaxPlayers = 2,
                    IsVisible = true,
                    IsOpen = true,
                }
            };
            MyPhoton.JoinRandomOrCreateRoom(joinRandomOrCreateRoomParameters);
        }
    }

    private void TriggerJoinedLobby()
    {
        JoinRoom();
    }

    private void TriggerJoinedRoom(Photon.Realtime.Room room)
    {
        
    }
}
