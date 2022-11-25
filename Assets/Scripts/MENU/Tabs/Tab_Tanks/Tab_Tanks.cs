public class Tab_Tanks : BaseTab_Tanks<Tab_StartGame>
{
    private Tab_SelectLobby _tabSelectLobby;



    protected override void Awake()
    {
        base.Awake();

        _tabSelectLobby = FindObjectOfType<Tab_SelectLobby>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        _object.onPlayOffline += OpenTab;
        _tabSelectLobby.onGoForward += OpenTab;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        _object.onPlayOffline -= OpenTab;
        _tabSelectLobby.onGoForward -= OpenTab;
    }
}
