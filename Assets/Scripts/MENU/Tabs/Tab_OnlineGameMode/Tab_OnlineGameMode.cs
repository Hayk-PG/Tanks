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
        _object.onOnlineModeTankSelected += base.OpenTab;
        _myPhotonCallbacks._OnLeftRoom += base.OpenTab;
    }

    private void OnDisable()
    {
        _object.onOnlineModeTankSelected -= base.OpenTab;
        _myPhotonCallbacks._OnLeftRoom -= base.OpenTab;
    }
}
