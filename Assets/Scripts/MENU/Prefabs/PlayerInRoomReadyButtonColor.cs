using UnityEngine;
using UnityEngine.UI;

public class PlayerInRoomReadyButtonColor : MonoBehaviour
{
    private PlayerInRoom _playerInRoom;

    [SerializeField] private Color[] _statusButtonBackgroundColor;
    [SerializeField] private Color[] _statusButtonFrameColor;
    [SerializeField] private Image _imageFrame, _imageBackground;


    private void Awake()
    {
        _playerInRoom = Get<PlayerInRoom>.From(gameObject);
    }

    private void Update()
    {
        ReadyButtonColor(_playerInRoom.IsPlayerReady);
    }

    public void ReadyButtonColor(bool isPlayerReady)
    {
        _imageBackground.color = isPlayerReady ? _statusButtonBackgroundColor[0] : _statusButtonBackgroundColor[1];
        _imageFrame.color = isPlayerReady ? _statusButtonFrameColor[0] : _statusButtonFrameColor[1];
    } 
}
