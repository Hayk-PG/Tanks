using UnityEngine;


public class StartGameButton : MonoBehaviour, IOnButtonInteract
{
    public GlobalOnButtonInteract GlobalOnButtonInteract { get; set; }

    public void StartTheGame()
    {
        MySceneManager.Instance.LoadScene(MySceneManager.SceneName.Game);
        GlobalOnButtonInteract?.OnChangeButtonSpriteAndColor(GlobalOnButtonInteract.ButtonState.Clicked);
    }
}
