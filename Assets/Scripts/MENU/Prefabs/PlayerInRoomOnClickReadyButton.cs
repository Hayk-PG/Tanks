using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerInRoomOnClickReadyButton : MonoBehaviour
{
    private PlayerInRoom _playerInRoom;
    private Network _netWork;
    private LoadLevel _loadLevel;



    private void Awake()
    {
        _playerInRoom = Get<PlayerInRoom>.From(gameObject);
        _netWork = FindObjectOfType<Network>();
        _loadLevel = FindObjectOfType<LoadLevel>();
    }

    private void OnEnable()
    {
        if (_netWork != null)
            _netWork.OnInvokeRPCMethode += OnPlayerReady;
    }

    private void OnDisable()
    {
        if (_netWork != null)
            _netWork.OnInvokeRPCMethode -= OnPlayerReady;
    }

    public void OnClickReadyButton()
    {
        if (_netWork != null && _playerInRoom.PlayerActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            _netWork.InvokeRPCMethode(PhotonNetwork.LocalPlayer);
            _loadLevel.Run(PhotonNetwork.LocalPlayer);
        }
    }

    private void OnPlayerReady(Player localPlayer)
    {
        if (_playerInRoom.PlayerActorNumber == localPlayer.ActorNumber)
        {           
            _playerInRoom.IsPlayerReady = !_playerInRoom.IsPlayerReady;
            CustomProperties.Add(localPlayer, Keys.IsPlayerReady, _playerInRoom.IsPlayerReady);            
        }
    }
}
