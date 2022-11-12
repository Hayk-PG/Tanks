public class Tab_Tanks : Tab_Base<Tab_StartGame>
{
    public CustomScrollRect CustomScrollRect { get; private set; }


    protected override void Awake()
    {
        base.Awake();
        CustomScrollRect = Get<CustomScrollRect>.FromChild(gameObject);
    }

    private void OnEnable()
    {
        _object.onPlayOffline += OpenTab;
    }

    private void OnDisable()
    {
        _object.onPlayOffline -= OpenTab;
    }

    public override void OpenTab()
    {
        base.OpenTab();
    }
}
