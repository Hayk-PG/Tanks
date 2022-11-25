public class Tab_HomeOnline : Tab_Base<MyPhotonCallbacks>
{
    protected override void OnEnable()
    {
        base.OnEnable();
        ExternalData.MyPlayfabRegistrationForm.onLogin += OpenTab;
        MenuTabs.Tab_SelectLobby.onGoBack += OpenTab;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        ExternalData.MyPlayfabRegistrationForm.onLogin -= OpenTab;
        MenuTabs.Tab_SelectLobby.onGoBack -= OpenTab;
    }
}
