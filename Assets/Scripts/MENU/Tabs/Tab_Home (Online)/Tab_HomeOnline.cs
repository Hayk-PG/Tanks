public class Tab_HomeOnline : Tab_Base<MyPhotonCallbacks>
{
    private IReset[] _iResets;

    private ConfirmTankBtn _confirmTankBtn;


    protected override void Awake()
    {
        base.Awake();

        _iResets = GetComponentsInChildren<IReset>();
        _confirmTankBtn = FindObjectOfType<ConfirmTankBtn>();
    }

    private void OnEnable()
    {
        ExternalData.MyPlayfabRegistrationForm.onLogin += OpenTab;
        _confirmTankBtn.onConfirmTankOnline += OpenTab;
    }

    private void OnDisable()
    {
        ExternalData.MyPlayfabRegistrationForm.onLogin -= OpenTab;
        _confirmTankBtn.onConfirmTankOnline -= OpenTab;
    }

    public override void OpenTab()
    {
        base.OpenTab();

        GlobalFunctions.Loop<IReset>.Foreach(_iResets, iReset => { iReset.SetDefault(); });
    }
}
