using UnityEngine;

public class PlayerInRoomReadyButtonColor : MonoBehaviour
{
    [SerializeField] private GameObject[] _buttonImages;
    private PlayerInRoom _playerInRoom;
    private bool _isSoundFxPlayed;
    


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
        ButtonImagesActivity(isPlayerReady);
    } 
    
    private void ButtonImagesActivity(bool isActive)
    {
        if (_buttonImages[0].activeInHierarchy != isActive)
        {
            _buttonImages[0].SetActive(isActive);

            if (_isSoundFxPlayed)
                _isSoundFxPlayed = false;
        }
            

        if (_buttonImages[1].activeInHierarchy == isActive)
        {
            _buttonImages[1].SetActive(isActive);
            SoundFX();
        }
    }

    private void SoundFX()
    {
        if (!_isSoundFxPlayed)
        {
            UISoundController.PlaySound(2, 0);
            _isSoundFxPlayed = true;
        }
    }
}
