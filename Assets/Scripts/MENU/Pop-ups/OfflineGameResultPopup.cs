using UnityEngine.SceneManagement;
using UnityEngine;


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
        if (PlayerPrefs.GetInt(Keys.GoOnlineReminder) > 0)
            return;

        MonoBehaviour.FindObjectOfType<Tab_PopUp>().Display(Tab_PopUp.PopUpType.OfflineModeReminder);
    }
}
