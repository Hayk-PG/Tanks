using UnityEngine.SceneManagement;

public class OfflineGameResultPopup 
{
    private string _txtGameResult;



    public void PrepareGameResultPopup(bool isWin)
    {
        _txtGameResult = isWin ? "You win" : "You lose";

        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    private void OnActiveSceneChanged(Scene scene1, Scene currentScene)
    {
        if(currentScene.name == MyScene.Manager.MenuSceneName)
        {
            Execute();

            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
        }
    }

    private void Execute()
    {
        GlobalFunctions.DebugLog(_txtGameResult);
    }
}
