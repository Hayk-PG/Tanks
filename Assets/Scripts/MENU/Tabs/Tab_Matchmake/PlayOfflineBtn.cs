public class PlayOfflineBtn : SubTabsButtonExpander
{
    protected override void Click()
    {
        MyScene.Manager.LoadScene(MyScene.SceneName.Game);
    }
}
