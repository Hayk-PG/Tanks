public class OptionsMainMenu : OptionsController
{
    protected override void Select()
    {
        OpenTabLoad();
        MyScene.Manager.LoadScene(MyScene.SceneName.Menu);
    }
}
