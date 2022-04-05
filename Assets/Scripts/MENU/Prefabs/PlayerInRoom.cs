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

    public struct Properties
    {
        public string _playerName;
        public bool _isActive;

        public Properties(string playerName, bool isActive)
        {
            _playerName = playerName;
            _isActive = isActive;
        }
    }


    public void DisplayProperties(Properties properties)
    {
        PlayerName = properties._playerName;
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, properties._isActive);
    }
}
