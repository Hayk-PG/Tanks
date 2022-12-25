using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

public class PhotonPlayerStatsTracker : MonoBehaviour
{
    private GameManager _gameManager;


    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void OnEnable()
    {
        _gameManager.OnGameStarted += OnGameStart;
    }

    private void OnDisable()
    {
        _gameManager.OnGameStarted -= OnGameStart;
    }

    private void OnGameStart()
    {
        RecordTimePlayed();
    }

    private void RecordTimePlayed()
    {
        Dictionary<string, int> timePlayed = new Dictionary<string, int> { { Keys.TimePlayed, 1 } };
        User.UpdateStats(Data.Manager.PlayfabId, timePlayed, null);
    }
}
