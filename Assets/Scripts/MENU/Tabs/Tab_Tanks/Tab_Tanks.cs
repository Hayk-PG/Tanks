public class Tab_Tanks : BaseTab_Tanks<Tab_StartGame>
{
    private SubTabTanksButton _subTabTanksButton;


    protected override void Awake()
    {
        base.Awake();

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
