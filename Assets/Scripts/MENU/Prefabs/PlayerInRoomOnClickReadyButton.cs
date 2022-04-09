using Photon.Realtime;
using UnityEngine;

public class PlayerInRoomOnClickReadyButton : MonoBehaviour
{
    private PlayerInRoom _playerInRoom;
    private Network _netWork;

    private void Awake()
    {
        _playerInRoom = Get<PlayerInRoom>.From(gameObject);
        _netWork = FindObjectOfType<Network>();
    }

    private void OnEnable()
    {
        if (_netWork != null) _netWork.OnInvokeRPCMethode += OnPlayerReady;
    }

    private void OnDisable()
    {
        if (_netWork != null) _netWork.OnInvokeRPCMethode -= OnPlayerReady;
    }

    public void OnClickReadyButton()
    {
        if (_netWork != null && _playerInRoom.PlayerActorNumber == MyPhotonNetwork.LocalPlayer.ActorNumber) _netWork.InvokeRPCMethode(MyPhotonNetwork.LocalPlayer);
    }

    private void OnPlayerReady(Player localPlayer)
    {
        if (_playerInRoom.PlayerActorNumber == localPlayer.ActorNumber)
        {
            _playerInRoom.IsPlayerReady = !_playerInRoom.IsPlayerReady;
            MyPlayerCustomProperties.UpdatePlayerCustomProperties(localPlayer, PlayerCustomPropertiesKeys.IsPlayerReady, _playerInRoom.IsPlayerReady);            
        }
    }
}
