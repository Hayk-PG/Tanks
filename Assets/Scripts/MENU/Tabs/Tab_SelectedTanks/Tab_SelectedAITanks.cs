
public class Tab_SelectedAITanks : Tab_Base<Tab_SelectedTanks>
{
    private void OnEnable()
    {
        _object.OnOfflineTankSelected += base.OpenTab;
    }

    private void OnDisable()
    {
        _object.OnOfflineTankSelected -= base.OpenTab;
    }

    public void OnClickSelectButton()
    {
        MyScene.Manager.LoadScene(MyScene.SceneName.Game);
    }
}
