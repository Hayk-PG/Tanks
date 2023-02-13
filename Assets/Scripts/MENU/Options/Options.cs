using System;
using UnityEngine;

public class Options : MonoBehaviour, IReset
{
    [SerializeField] 
    private CanvasGroup _canvasGroup;

    [SerializeField] [Space]
    private Btn _btnHome;

    [SerializeField] [Space]
    private Btn _btnGameMode;

    [SerializeField] [Space]
    private Btn _btnLogOut;

    private IReset[] _iResets;

    public event Action<bool> onOptionsActivity;

    public MyPhotonCallbacks MyPhotonCallbacks { get; private set; }


    private void Awake()
    {
        _iResets = GetComponentsInChildren<IReset>();
        MyPhotonCallbacks = FindObjectOfType<MyPhotonCallbacks>();
    }

    public void Activity(bool isActive)
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, isActive);
        GlobalFunctions.Loop<IReset>.Foreach(_iResets, iReset => { iReset?.SetDefault(); });
        onOptionsActivity?.Invoke(isActive);
    }

    public void SetDefault()
    {
        if(MyScene.Manager.CurrentScene.name == MyScene.Manager.GameSceneName)
        {
            SetBtnActivity(_btnHome.gameObject, true);
            SetBtnActivity(_btnGameMode.gameObject, false);
            SetBtnActivity(_btnLogOut.gameObject, false);
        }

        if (MyScene.Manager.CurrentScene.name == MyScene.Manager.MenuSceneName)
        {
            print("aa");
            SetBtnActivity(_btnHome.gameObject, false);
            SetBtnActivity(_btnGameMode.gameObject, true);
            SetBtnActivity(_btnLogOut.gameObject, !MyPhotonNetwork.IsOfflineMode);
        }
    }

    private void SetBtnActivity(GameObject gameObject, bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
