public class Tab_SelectLobby : Tab_Base<Tab_HomeOnline>
{
    protected override void OnEnable()
    {
        base.OnEnable();

        _object.onGoForward += OpenTab;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        _object.onGoForward -= OpenTab;
    }
}
