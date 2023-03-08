using System;
using System.Collections;
using UnityEngine;

public class Tab_Initialize : Tab_Base
{
    public event Action onOpenTabStartGame;
    public event Action onJumpTabSignUp;
    public event Action onJumpTabOffline;


    private void Start() => StartCoroutine(Execute());

    private IEnumerator Execute()
    {
        _tabLoading.Open();
        MyPhoton.Disconnect();
        yield return new WaitUntil(() => !MyPhotonNetwork.IsConnected);
        Conditions<bool>.Compare(MyPhoton.GameModeRegistered == MyPhoton.RegisteredGameMode.None, OpenTabStartGame, SkipTabStartGame);
    }

    private void OpenTabStartGame() => onOpenTabStartGame?.Invoke();

    private void SkipTabStartGame()
    {
        if (MyPhoton.GameModeRegistered == MyPhoton.RegisteredGameMode.Online)
        {
            MyPhotonNetwork.ManageOfflineMode(false);
            onJumpTabSignUp?.Invoke();
        }
        else
        {
            MyPhotonNetwork.ManageOfflineMode(true);
            onJumpTabOffline?.Invoke();
        }
    }
}
