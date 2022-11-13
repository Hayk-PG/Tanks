public class Tab_Tanks : Tab_Base<Tab_StartGame>
{
    public CustomScrollRect CustomScrollRect { get; private set; }
    private SubTabTanksButton _subTabTanksButton;


    protected override void Awake()
    {
        base.Awake();
        CustomScrollRect = Get<CustomScrollRect>.FromChild(gameObject);
        _subTabTanksButton = FindObjectOfType<SubTabTanksButton>();
    }

    private void OnEnable()
    {
        _object.onPlayOffline += OpenTab;
        _subTabTanksButton.onOpenTanksTab += OpenTab;
    }

    private void OnDisable()
    {
        _object.onPlayOffline -= OpenTab;
        _subTabTanksButton.onOpenTanksTab -= OpenTab;
    }

    public override void OpenTab()
    {
        base.OpenTab();
    }
}
