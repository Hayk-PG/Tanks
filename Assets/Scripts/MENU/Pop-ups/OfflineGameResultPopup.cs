using UnityEngine.SceneManagement;

public class OfflineGameResultPopup 
{
    public void PrepareGameResultPopup()
    {
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
        UnityEngine.MonoBehaviour.FindObjectOfType<Tab_PopUp>().Display(Tab_PopUp.PopUpType.OfflineModeReminder);
    }
}
