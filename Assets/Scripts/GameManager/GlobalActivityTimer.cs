using Photon.Pun;
using UnityEngine;

public class GlobalActivityTimer : MonoBehaviourPun
{
    private MyPlugins _myPlugins;
    private PlayerShields[] _playerShields;
    public int[] _playersActiveShieldsTimer;
    

    private void Awake()
    {
        _myPlugins = FindObjectOfType<MyPlugins>();
        _playersActiveShieldsTimer = new int[2];
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
        for (int i = 0; i < _playersActiveShieldsTimer.Length; i++)
        {
            if (_playersActiveShieldsTimer[i] > 0)
                _playersActiveShieldsTimer[i]--;
        }

        if (_playersActiveShieldsTimer[0] == 0)
        {
            photonView.RPC("ShieldsActivityRPC", RpcTarget.AllViaServer, TurnState.Player1);
            _playersActiveShieldsTimer[0] = -1;
        }

        if (_playersActiveShieldsTimer[1] == 0)
        {
            photonView.RPC("ShieldsActivityRPC", RpcTarget.AllViaServer, TurnState.Player2);
            _playersActiveShieldsTimer[1] = -1;
        }
    }

    [PunRPC]
    private void ShieldsActivityRPC(TurnState turnState)
    {
        PlayerShields playerShields = GlobalFunctions.ObjectsOfType<PlayerShields>.Find(shield => Get<PlayerTurn>.From(shield.gameObject).MyTurn == turnState);
        playerShields?.DeactivateShields();
    }
}
