public class OptionsMainMenu : OptionsController
{
    protected override void Select()
    {
        MyScene.Manager.LoadScene(MyScene.SceneName.Menu);
    }
}
