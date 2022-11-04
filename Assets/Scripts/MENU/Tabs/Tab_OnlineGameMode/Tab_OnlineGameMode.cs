public class Tab_OnlineGameMode : Tab_Base<Tab_SelectedTanks>
{
    private MyPhotonCallbacks _myPhotonCallbacks;


    protected override void Awake()
    {
        base.Awake();

        _myPhotonCallbacks = FindObjectOfType<MyPhotonCallbacks>();
    }

    private void OnEnable()
    {
        _object.onOnlineModeTankSelected += OpenTab;
        _myPhotonCallbacks._OnLeftRoom += OpenTab;
        _myPhotonCallbacks._OnLeftLobby += OpenTab;
    }

    private void OnDisable()
    {
        _object.onOnlineModeTankSelected -= OpenTab;
        _myPhotonCallbacks._OnLeftRoom -= OpenTab;
        _myPhotonCallbacks._OnLeftLobby -= OpenTab;
    }

    public override void OpenTab()
    {
        base.OpenTab();

        if (MyPhotonNetwork.IsInLobby)
        {
            MyPhoton.LeaveLobby();
        }
    }
}
