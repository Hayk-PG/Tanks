using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyScene : MonoBehaviour
{   
    public enum SceneName { Init, Menu, Game}

    [HideInInspector]
    public SceneName _sceneName;

    public static MyScene Manager { get; private set; }
    public string InitSceneName { get; private set; } = "Init";
    public string MenuSceneName { get; private set; } = "Menu";
    public string GameSceneName { get; private set; } = "Game";
    public Scene CurrentScene { get => SceneManager.GetActiveScene(); }

    public Action OnDestroyOnLoadMenuScene { get; set; }


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
            case SceneName.Menu:                 

                if (CurrentScene.name != InitSceneName)
                {
                    OnDestroyOnLoadMenuScene?.Invoke();
                    SceneManager.LoadScene(InitSceneName);
                }
                else
                {
                    SceneManager.LoadScene(MenuSceneName);
                }
                break;

            case SceneName.Game: 
                SceneManager.LoadScene(GameSceneName); 
                break;
        }
    }
}
