using UnityEngine;


public class LobbyCell : MonoBehaviour
{
    [SerializeField]
    private RoundTimeStates _roundTimeStates;

    [SerializeField] [Space]
    private GameWind _gameWind;

    [SerializeField] [Space]
    private LobbyItem[] _lobbyItemsEntryFee, _lobbyItemsWin, _lobbyItemsLose;

    [SerializeField] [Space]
    private LobbyGUI _lobbyGUI;

    [SerializeField] [Space]
    private Btn _btnJoinLobby;

    [SerializeField] [Space]
    private Maps _allMaps;

    [SerializeField] [Space]
    private string _lobbyName;

    [SerializeField] [Space]
    private int _mapIndex;

    [SerializeField] [Space]
    private Color _color;

    private MyPhotonCallbacks _myPhotonCallbacks;
    private TabLoading _tabLoading;

    private string LobbyName
    {
        get => _lobbyName;
    }
    private Color Color
    {
        get => _color;
    }
    private int RoundDuration
    {
        get => Data.Manager.ConvertRoundTimeStates(_roundTimeStates);
    }

    public int MapIndex
    {
        get
        {
            return _mapIndex < _allMaps.All.Length ? _mapIndex : _allMaps.All.Length - 1;
        }
    }
    private bool IsWindOn
    {
        get => _gameWind == global::GameWind.On ? true : false;
    }



    private void Awake()
    {
        _myPhotonCallbacks = FindObjectOfType<MyPhotonCallbacks>();

        _tabLoading = Get<TabLoading>.FromChild(MenuTabs.Tab_SelectLobby.gameObject);
    }

    private void Start()
    {
        _lobbyGUI.PrintLobbyName(LobbyName);

        _lobbyGUI.PrintRoundDuration(RoundDuration);

        _lobbyGUI.SetColor(Color);

        _lobbyGUI.SetWindBlockActivity(IsWindOn);
    }

    private void OnEnable()
    {
        _btnJoinLobby.onSelect += JoinLobby;

        _myPhotonCallbacks._OnJoinedLobby += CompleteJoinRoom;
    }

    private void OnDisable()
    {
        _btnJoinLobby.onSelect -= JoinLobby;

        _myPhotonCallbacks._OnJoinedLobby -= CompleteJoinRoom;
    }

    private void JoinLobby()
    {
        MyPhoton.JoinLobby(LobbyName, Photon.Realtime.LobbyType.AsyncRandomLobby);

        _tabLoading.Open();
    }

    private void JoinRandomOrCreateRoom()
    {
        if (MyPhotonNetwork.CurrentLobby.Name == LobbyName)
        {
            string[] entryFee = new string[]
            {
                _lobbyItemsEntryFee[0].Type,
                _lobbyItemsEntryFee[1].Type,
                _lobbyItemsEntryFee[2].Type,
            };
            string[] win = new string[]
            {
                _lobbyItemsWin[0].Type,
                _lobbyItemsWin[1].Type,
                _lobbyItemsWin[2].Type,
            };
            string[] lose = new string[]
            {
                _lobbyItemsLose[0].Type,
                _lobbyItemsLose[1].Type,
                _lobbyItemsLose[2].Type
            };
            int[] entryFeeAmount = new int[]
            {
                _lobbyItemsEntryFee[0].Quantity,
                _lobbyItemsEntryFee[1].Quantity,
                _lobbyItemsEntryFee[2].Quantity,
            };
            int[] winAmount = new int[]
            {
                _lobbyItemsWin[0].Quantity,
                _lobbyItemsWin[1].Quantity,
                _lobbyItemsWin[2].Quantity,
            };
            int[] loseAmount = new int[]
            {
                _lobbyItemsLose[0].Quantity,
                _lobbyItemsLose[1].Quantity,
                _lobbyItemsLose[2].Quantity
            };
            string[][] itemsName = new string[][] { entryFee, win, lose };

            int[][] itemsAmount = new int[][] { entryFeeAmount, winAmount, loseAmount };

            MyPhoton.JoinRandomOrCreateRoomParameters joinRandomOrCreateRoomParameters = new MyPhoton.JoinRandomOrCreateRoomParameters
            {
                TypedLobby = new Photon.Realtime.TypedLobby(LobbyName, Photon.Realtime.LobbyType.Default),

                ExpectedMaxPlayers = 2,

                MatchmakingMode = Photon.Realtime.MatchmakingMode.FillRoom,

                RoomOptions = new Photon.Realtime.RoomOptions
                {
                    CleanupCacheOnLeave = true,

                    MaxPlayers = 2,

                    CustomRoomProperties = new ExitGames.Client.Photon.Hashtable {
                        { Keys.RoomFakeName, LobbyName },
                        { Keys.MapWind, IsWindOn },
                        { Keys.RoundTime, RoundDuration },
                        { Keys.MapIndex, MapIndex },
                        { Keys.ItemName,  itemsName},
                        { Keys.ItemAmount,  itemsAmount}
                    },
                    IsVisible = true,
                    IsOpen = true,
                },
            };

            MyPhoton.JoinRandomOrCreateRoom(joinRandomOrCreateRoomParameters);
        }
    }

    private void CompleteJoinRoom() => JoinRandomOrCreateRoom();
}
