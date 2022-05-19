using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInRoom : MonoBehaviour
{
    [SerializeField] CanvasGroup _canvasGroup;
    [SerializeField] private Text _text_playerName;
    [SerializeField] private Badge _badge;

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
        public int _playerLevel;
        public bool _isPlayerReady;

        public Properties(string playerName, int actorNumber, int playerLevel, bool isPlayerReady)
        {
            _playerName = playerName;
            _actorNumber = actorNumber;
            _playerLevel = playerLevel;
            _isPlayerReady = isPlayerReady;
        }
    }

    public void AssignPlayerInRoom(Properties properties)
    {
        SetObjectName(properties._actorNumber.ToString());
        SetPlayerInformation(properties._playerName, properties._actorNumber, properties._playerLevel, properties._isPlayerReady);
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, true);
    }

    public void ResetPlayerInRoom()
    {
        SetObjectName("PlayerInRoom");
        SetPlayerInformation("", 0, 0, false);
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, false);
    }

    private void SetObjectName(string name)
    {
        this.name = name;
    }

    private void SetPlayerInformation(string name, int actorNumber,int playerLevel, bool isPlayerReady)
    {
        PlayerName = name;
        PlayerActorNumber = actorNumber;
        _badge.Set(playerLevel);
        IsPlayerReady = isPlayerReady;
    }
}
