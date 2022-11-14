public class Tab_Matchmake : Tab_Base<MyPhotonCallbacks>
{
    private IReset[] _iResets;


    protected override void Awake()
    {
        base.Awake();

        _iResets = GetComponentsInChildren<IReset>();
    }

    private void OnEnable()
    {
        _object._OnJoinedLobby += OpenTab;
    }

    private void OnDisable()
    {
        _object._OnJoinedLobby -= OpenTab;
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
