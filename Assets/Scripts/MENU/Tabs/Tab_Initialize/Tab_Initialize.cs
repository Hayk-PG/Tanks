using System.Collections;
using UnityEngine;

public class Tab_Initialize : Tab_Base
{
    private void Start() => StartCoroutine(Execute());

    private IEnumerator Execute()
    {
        MyPhoton.Disconnect();

        _tabLoading.Open();

        yield return new WaitUntil(() => !MyPhotonNetwork.IsConnected);

        Conditions<bool>.Compare(MyPhoton.GameModeRegistered == MyPhoton.RegisteredGameMode.None, OpenTabStartGame, SkipTabStartGame);
    }

    private void OpenTabStartGame() => TabsOperation.Handler.SubmitOperation(this, TabsOperation.Operation.Start);

    private void SkipTabStartGame()
    {
        if (MyPhoton.GameModeRegistered == MyPhoton.RegisteredGameMode.Online)
            TabsOperation.Handler.SubmitOperation(this, TabsOperation.Operation.PlayOnline);
        else
            TabsOperation.Handler.SubmitOperation(this, TabsOperation.Operation.PlayOffline);
    }
}
