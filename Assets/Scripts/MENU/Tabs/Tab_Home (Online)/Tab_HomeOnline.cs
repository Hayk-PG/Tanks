public class Tab_HomeOnline : Tab_Base<MyPhotonCallbacks>
{
    private IReset[] _iResets;


    protected override void Awake()
    {
        base.Awake();

        _iResets = GetComponentsInChildren<IReset>();
    }

    private void OnEnable()
    {
        ExternalData.MyPlayfabRegistrationForm.onLogin += OpenTab;
    }

    private void OnDisable()
    {
        ExternalData.MyPlayfabRegistrationForm.onLogin -= OpenTab;
    }

    public override void OpenTab()
    {
        base.OpenTab();

        GlobalFunctions.Loop<IReset>.Foreach(_iResets, iReset => { iReset.SetDefault(); });
    }
}
