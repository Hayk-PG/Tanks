public class Tab_Matchmake : Tab_Base<MyPhotonCallbacks>
{
    private IReset[] _iResets;
    private Tab_SelectedAITanks _tabSelectAITanks;


    protected override void Awake()
    {
        base.Awake();

        _iResets = GetComponentsInChildren<IReset>();
        _tabSelectAITanks = FindObjectOfType<Tab_SelectedAITanks>();
    }

    private void OnEnable()
    {
        _object._OnJoinedLobby += OpenTab;
        _tabSelectAITanks.OnAITankSelected += OpenTab;
    }

    private void OnDisable()
    {
        _object._OnJoinedLobby -= OpenTab;
        _tabSelectAITanks.OnAITankSelected -= OpenTab;
    }

    public override void OpenTab()
    {
        base.OpenTab();
        ResetSubElements();
    }

    private void ResetSubElements()
    {
        GlobalFunctions.Loop<IReset>.Foreach(_iResets, iReset => { iReset.SetDefault(); });
    }
}
