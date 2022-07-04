using Photon.Pun;
using UnityEngine;

public class GlobalActivityTimer : MonoBehaviourPun
{
    private MyPlugins _myPlugins;
    private PlayerShields[] _playerShields;
    public int[] PlayersActiveShieldsTimer { get; set; } = new int[2];
    

    private void Awake()
    {
        _myPlugins = FindObjectOfType<MyPlugins>();        
    }

    private void OnEnable()
    {
        _myPlugins.OnPluginService += OnPluginService;        
    }

    private void OnDisable()
    {
        _myPlugins.OnPluginService -= OnPluginService;
    }

    private void OnPluginService()
    {
        if (MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer))
        {
            ShieldsActivity();
        }
    }

    private void ShieldsActivity()
    {
        for (int i = 0; i < PlayersActiveShieldsTimer[i]; i++)
        {
            if (PlayersActiveShieldsTimer[i] > 0)
                PlayersActiveShieldsTimer[i]--;
        }

        if (PlayersActiveShieldsTimer[0] == 0)
        {
            photonView.RPC("ShieldsActivityRPC", RpcTarget.AllViaServer, TurnState.Player1);
            PlayersActiveShieldsTimer[0] = -1;
        }

        if (PlayersActiveShieldsTimer[1] == 0)
        {
            photonView.RPC("ShieldsActivityRPC", RpcTarget.AllViaServer, TurnState.Player2);
            PlayersActiveShieldsTimer[0] = -1;
        }
    }

    [PunRPC]
    private void ShieldsActivityRPC(TurnState turnState)
    {
        PlayerShields playerShields = GlobalFunctions.ObjectsOfType<PlayerShields>.Find(shield => Get<PlayerTurn>.From(shield.gameObject).MyTurn == turnState);
        playerShields?.DeactivateShields();
    }
}
