public class Tab_HomeOnline : Tab_Base
{
    private MyPhotonCallbacks _myPhotonCallbacks;


    protected override void Awake()
    {
        base.Awake();
        _myPhotonCallbacks = FindObjectOfType<MyPhotonCallbacks>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _myPhotonCallbacks._OnConnectedToMaster += OpenTab;
        MenuTabs.Tab_SelectLobby.onGoBack += OpenTab;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _myPhotonCallbacks._OnConnectedToMaster -= OpenTab;
        MenuTabs.Tab_SelectLobby.onGoBack -= OpenTab;
    }
}
