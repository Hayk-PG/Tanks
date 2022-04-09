using UnityEngine;
using UnityEngine.SceneManagement;

public class MyScene : MonoBehaviour
{   
    public enum SceneName { Menu, Game}

    [HideInInspector]
    public SceneName _sceneName;

    public static MyScene Manager { get; private set; }
    public string MenuSceneName { get; private set; } = "Menu";
    public string GameSceneName { get; private set; } = "Game";
    public Scene CurrentScene { get => SceneManager.GetActiveScene(); }




    private void Awake()
    {
        if(Manager == null)
        {
            Manager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(SceneName sceneName)
    {
        switch (sceneName)
        {
            case SceneName.Menu: SceneManager.LoadScene(MenuSceneName); break;
            case SceneName.Game: SceneManager.LoadScene(GameSceneName); break;
        }
    }
}
