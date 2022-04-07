using UnityEngine;
using UnityEngine.UI;

public class PlayerInRoom : MonoBehaviour
{
    [SerializeField] CanvasGroup _canvasGroup;
    [SerializeField] private Text _text_playerName;

    public string PlayerName
    {
        get => _text_playerName.text;
        private set => _text_playerName.text = value;
    }
    public int PlayerActorNumber { get; private set; }
    public bool IsPlayerReady { get; set; }

    public struct Properties
    {
        public string _playerName;
        public int _actorNumber;
        public bool _isActive;

        public Properties(string playerName, int actorNumber, bool isActive)
        {
            _playerName = playerName;
            _actorNumber = actorNumber;
            _isActive = isActive;
        }
    }


    public void AssignPlayerInRoom(Properties properties)
    {
        name = properties._actorNumber.ToString();

        PlayerName = properties._playerName;
        PlayerActorNumber = properties._actorNumber;
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, properties._isActive);
    }

    public void Hide()
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, false);
    }
}
