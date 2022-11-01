
public class Tab_SelectMaps : Tab_Base<MyPhotonCallbacks>
{
    private Tab_SelectMapOnline _tabSelectMapOnline;
    private Tab_SelectedAITanks _tabSelectedAiTanks;
    private Tab_CreateRoom _tabCreateRoom;
    private Tab_Lobby _tabLobby;


    protected override void Awake()
    {
        base.Awake();
        _tabSelectMapOnline = Get<Tab_SelectMapOnline>.From(gameObject);
        _tabSelectedAiTanks = FindObjectOfType<Tab_SelectedAITanks>();
        _tabCreateRoom = FindObjectOfType<Tab_CreateRoom>();
        _tabLobby = FindObjectOfType<Tab_Lobby>();
    }

    private void OnEnable()
    {
        //_tabSelectedAiTanks.OnAITankSelected += base.OpenTab;
        _tabCreateRoom.OnOpenTab_SelectMap += OpenTabThroughRoomCreation;
    }

    private void OnDisable()
    {
        //_tabSelectedAiTanks.OnAITankSelected -= base.OpenTab;
        _tabCreateRoom.OnOpenTab_SelectMap -= OpenTabThroughRoomCreation;
    }

    private void OpenTabThroughRoomCreation(string roomName, string password, bool isPasswordSet)
    {
        base.OpenTab();
        _tabSelectMapOnline._roomProperties = new Tab_SelectMapOnline.RoomProperties { _roomName = roomName, _password = password, _isPasswordSet = isPasswordSet }; 
    }

    public void ConfirmSelectedMap()
    {
        if (MyPhotonNetwork.IsOfflineMode)
        {
            MyScene.Manager.LoadScene(MyScene.SceneName.Game);
        }
        else
        {
            MyPhoton.CreateRoom( Photon.Realtime.LobbyType.Default, "", _tabSelectMapOnline._roomProperties._roomName, _tabSelectMapOnline._roomProperties._password, _tabSelectMapOnline._roomProperties._isPasswordSet, Data.Manager.MapIndex, Data.Manager.GameTime, Data.Manager.IsWindOn);
        }

        Loading.Activity(true);
    }

    public void OnTopBarBackButton()
    {
        if (MyPhotonNetwork.IsOfflineMode)
        {
            _tabSelectedAiTanks.OpenTab();
        }
        else
        {
            _tabLobby.OpenTab();
        }
    }
}
