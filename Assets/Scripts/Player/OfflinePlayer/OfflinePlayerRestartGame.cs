using UnityEngine;

public class OfflinePlayerRestartGame : MonoBehaviour
{
    private Tab_EndgameRematchButton _tabEndGameRematchButton;


    private void Awake()
    {
        _tabEndGameRematchButton = FindObjectOfType<Tab_EndgameRematchButton>();
    }

    private void OnEnable()
    {
        _tabEndGameRematchButton.OnRematch += RestartGame;
    }

    private void OnDisable()
    {
        _tabEndGameRematchButton.OnRematch -= RestartGame;
    }

    private void RestartGame()
    {
        MyScene.Manager.LoadScene(MyScene.SceneName.Game);
    }
}
