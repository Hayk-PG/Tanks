using UnityEngine;


public class StartGameButton : MonoBehaviour, IOnButtonInteract
{
    public Btn GlobalOnButtonInteract { get; set; }

    public void StartTheGame()
    {
        MyScene.Manager.LoadScene(MyScene.SceneName.Game);
        //GlobalOnButtonInteract?.OnChangeButtonSpriteAndColor(CustomButton.ButtonState.Clicked);
    }
}
