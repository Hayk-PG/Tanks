using UnityEngine;
using UnityEngine.UI;

public class RoomButton : MonoBehaviour
{
    [SerializeField] private Text _text_roomNameText;
    [SerializeField] private Text _text_additionalInfoText;
    [SerializeField] private Text _text_playersCountText;
    [SerializeField] private Image _imageMap;
    [SerializeField] private Maps _maps;
    public struct UI
    {
        public string _roomName;
        public string _additionalInfo;
        public string _playersCount;
        public int _mapIndex;

        public UI(string roomName, string additionalInfo, string playersCount, int mapIndex)
        {
            _roomName = roomName;
            _additionalInfo = additionalInfo;
            _playersCount = playersCount;
            _mapIndex = mapIndex;
        }
    }

    public void AssignRoomButton(UI ui)
    {
        name = ui._roomName;

        _text_roomNameText.text = ui._roomName;
        _text_additionalInfoText.text = ui._additionalInfo;
        _text_playersCountText.text = ui._playersCount;
        _imageMap.sprite = _maps.All[ui._mapIndex].MapImage;
    }

    public void OnClickToJoin()
    {
        MyPhoton.JoinRoom(_text_roomNameText.text);
    }
}
