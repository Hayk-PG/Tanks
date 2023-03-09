using System.Collections;
using UnityEngine;

public class Tab_HomeOnline : Tab_Base
{
    protected override void OnEnable()
    {
        base.OnEnable();

        MenuTabs.Tab_SelectLobby.onGoBack += OpenTab;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        MenuTabs.Tab_SelectLobby.onGoBack -= OpenTab;
    }

    protected override void OnTaskReceived(ITabOperation handler, TabsOperation.Operation operation, object[] data)
    {
        if(operation == TabsOperation.Operation.OpenOnlineTab)
        {
            OperationHandler = handler;

            StartCoroutine(Execute((string)data[0], (string)data[1]));
        }
    }

    private IEnumerator Execute(string photonNetworkNickname, string photonNetworkUserId)
    {
        MyPhoton.Connect(photonNetworkNickname, photonNetworkUserId);

        float waitTime = 5f;
        float elapsedTime = 0f;

        while(elapsedTime < waitTime)
        {
            elapsedTime += Time.deltaTime;

            print($"Tab_HomeOnlone execute... {elapsedTime:0}");

            if (MyPhotonNetwork.IsConnected)
            {
                yield return new WaitForSeconds(1f);

                OpenTab();

                elapsedTime = waitTime;

                yield break;
            }

            if (elapsedTime >= waitTime)
            {
                MyPhoton.Disconnect();

                OperationHandler?.OnOperationFailed();
            }

            yield return null;
        }
    }

    public override void OpenTab()
    {
        base.OpenTab();

        MyPhoton.GameModeRegistered = MyPhoton.RegisteredGameMode.Online;
    }
}
