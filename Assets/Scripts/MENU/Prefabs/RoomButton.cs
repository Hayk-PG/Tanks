using UnityEngine;
using UnityEngine.UI;

public class RoomButton : MonoBehaviour
{
    [SerializeField] private Text _text_roomNameText;
    [SerializeField] private Text _text_additionalInfoText;
    [SerializeField] private Text _text_playersCountText;

    [SerializeField] private Image _image_roomIcon;


    public struct UI
    {
        public string _roomName;
        public string _additionalInfo;
        public string _playersCount;

        public Sprite _roomIcon;

        public UI(string roomName, string additionalInfo, string playersCount, Sprite roomIcon)
        {
            _roomName = roomName;
            _additionalInfo = additionalInfo;
            _playersCount = playersCount;

            _roomIcon = roomIcon;
        }
    }

    public void AssignRoomButton(UI ui)
    {
        name = ui._roomName;

        _text_roomNameText.text = ui._roomName;
        _text_additionalInfoText.text = ui._additionalInfo;
        _text_playersCountText.text = ui._playersCount;

        if(ui._roomIcon != null) _image_roomIcon.sprite = ui._roomIcon;
    }

    public void OnClickToJoin()
    {
        MyPhoton.JoinRoom(_text_roomNameText.text);
    }
}
