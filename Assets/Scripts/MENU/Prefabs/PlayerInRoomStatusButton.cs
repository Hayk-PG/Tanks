using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PlayerInRoomStatusButton : MonoBehaviour
{
    private PlayerInRoom _playerInRoom;
    private Network _netWork;

    [SerializeField] private Color[] _statusButtonBackgroundColor;
    [SerializeField] private Color[] _statusButtonFrameColor;
    [SerializeField] private Image _imageFrame, _imageBackground;


    private void Awake()
    {
        _playerInRoom = Get<PlayerInRoom>.From(gameObject);
        _netWork = FindObjectOfType<Network>();
    }

    private void OnEnable()
    {
        if(_netWork != null) _netWork.OnInvokeRPCMethode += RPC_Status;
    }

    private void OnDisable()
    {
        if (_netWork != null) _netWork.OnInvokeRPCMethode -= RPC_Status;
    }

    public void OnClickStatusButton()
    {
        if (_netWork != null && _playerInRoom.PlayerActorNumber == PhotonNetwork.LocalPlayer.ActorNumber) _netWork.InvokeRPCMethode(PhotonNetwork.LocalPlayer);
    }

    private void RPC_Status(Player localPlayer)
    {
        if(_playerInRoom.PlayerActorNumber == localPlayer.ActorNumber)
        {
            _playerInRoom.IsPlayerReady = !_playerInRoom.IsPlayerReady;

            _imageBackground.color = _playerInRoom.IsPlayerReady ? _statusButtonBackgroundColor[0] : _statusButtonBackgroundColor[1];
            _imageFrame.color = _playerInRoom.IsPlayerReady ? _statusButtonFrameColor[0] : _statusButtonFrameColor[1];

            print(_playerInRoom.PlayerActorNumber);
        }
    }
}
