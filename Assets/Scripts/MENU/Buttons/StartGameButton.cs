using UnityEngine;


public class StartGameButton : MonoBehaviour, IOnButtonInteract
{
    public GlobalOnButtonInteract GlobalOnButtonInteract { get; set; }

    public void StartTheGame()
    {
        MyScene.Manager.LoadScene(MyScene.SceneName.Game);
        GlobalOnButtonInteract?.OnChangeButtonSpriteAndColor(GlobalOnButtonInteract.ButtonState.Clicked);
    }
}
