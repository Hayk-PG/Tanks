using UnityEngine;
using System.Collections.Generic;

public class PhotonPlayerStatsTracker : MonoBehaviour
{
    private GameManager _gameManager;
    private GameResultProcessor _gameResultProcessor;

    private bool _areStartDatasUpdated;
    private bool _areEndDatasUpdated;


    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _gameResultProcessor = FindObjectOfType<GameResultProcessor>();
    }

    private void OnEnable()
    {
        _gameManager.OnGameStarted += OnGameStart;
        _gameResultProcessor.onFinishResultProcess += OnGameEndResult;                
    }

    private void OnDisable()
    {
        _gameManager.OnGameStarted -= OnGameStart;
        _gameResultProcessor.onFinishResultProcess -= OnGameEndResult;
    }

    private void OnGameStart()
    {
        if(!_areStartDatasUpdated)
        {
            UpdateTimePlayed();
            _areStartDatasUpdated = true;
        }
    }

    private void OnGameEndResult()
    {
        if (!_areEndDatasUpdated)
        {
            UpdateUserStats();
            _areEndDatasUpdated = true;
        }
    }

    private void UpdateTimePlayed()
    {
        Dictionary<string, int> timePlayed = new Dictionary<string, int> { { Keys.TimePlayed, 1 } };
        User.UpdateStats(Data.Manager.PlayfabId, timePlayed, null);
    }

    private void UpdateUserStats()
    {

    }
}
