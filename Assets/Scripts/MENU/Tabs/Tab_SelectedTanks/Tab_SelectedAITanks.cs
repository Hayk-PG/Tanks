
public class Tab_SelectedAITanks : Tab_Base<Tab_SelectedTanks>
{
    private void OnEnable()
    {
        _object.OnSingleGameTankSelected += OpenTab;
    }

    private void OnDisable()
    {
        _object.OnSingleGameTankSelected -= OpenTab; 
    }

    public override void OpenTab()
    {
        base.OpenTab();
    }

    public void OnClickSelectButton()
    {
        MyScene.Manager.LoadScene(MyScene.SceneName.Game);
    }
}
