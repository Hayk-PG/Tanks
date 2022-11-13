public class Tab_HomeOnline : Tab_Base<MyPhotonCallbacks>
{
    private IReset[] _iResets;

    private TabTankConfirmButton _confirmTankButton;


    protected override void Awake()
    {
        base.Awake();

        _iResets = GetComponentsInChildren<IReset>();
        _confirmTankButton = FindObjectOfType<TabTankConfirmButton>();
    }

    private void OnEnable()
    {
        ExternalData.MyPlayfabRegistrationForm.onLogin += OpenTab;
        _confirmTankButton.onConfirmTankOnline += OpenTab;
    }

    private void OnDisable()
    {
        ExternalData.MyPlayfabRegistrationForm.onLogin -= OpenTab;
        _confirmTankButton.onConfirmTankOnline -= OpenTab;
    }

    public override void OpenTab()
    {
        base.OpenTab();

        GlobalFunctions.Loop<IReset>.Foreach(_iResets, iReset => { iReset.SetDefault(); });
    }
}
