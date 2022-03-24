using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public enum TurnState { None, Transition, Player1, Player2, Other}

public class TurnController : MonoBehaviour
{
    public TurnState _turnState;
    public TurnState _previousTurnState;

    private GameManager _gameManager;
    private CameraMovement _cameraMovement;

    private List<PlayerTurn> _playersTurn;

    public event Action<TurnState,CameraMovement> OnTurnChanged;

    /// <summary>
    /// 0: PlayerOne 1:PlayerTwo
    /// </summary>
    public static List<PlayerTurn> Players { get; set; } = new List<PlayerTurn>();
    

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _cameraMovement = FindObjectOfType<CameraMovement>();
    }

    private void OnEnable()
    {
        _gameManager.OnGameStarted += SetPlayersTurnOnGameStart;
    }

    private void OnDisable()
    {
        _gameManager.OnGameStarted -= SetPlayersTurnOnGameStart;
    }

    private void SharePlayersTurnOrder()
    {
        _playersTurn = FindObjectsOfType<PlayerTurn>().ToList();

        for (int i = 0; i < _playersTurn.Count; i++)
        {
            if(_playersTurn[i].transform.position.x < 0)
            {
                _playersTurn[i].MyTurn = TurnState.Player1;
                Players.Add(_playersTurn[i]);
            }
            else
            {
                _playersTurn[i].MyTurn = TurnState.Player2;
                Players.Add(_playersTurn[i]);
            }
        }
    }

    private void SetPlayersTurnOnGameStart()
    {
        SharePlayersTurnOrder();
        SetNextTurn(RandomTurn());
    }

    private TurnState RandomTurn()
    {
        int randomTurn = UnityEngine.Random.Range(0, 2);
        _turnState = randomTurn == 0 ? TurnState.Player1 : TurnState.Player2;

        return _turnState;
    }

    public void SetNextTurn(TurnState turnState)
    {
        if(_turnState == TurnState.Player1 || _turnState == TurnState.Player2) _previousTurnState = _turnState;
        _turnState = turnState;

        Invoke("NextTurnFromTransition", 2);
        OnTurnChanged?.Invoke(_turnState, _cameraMovement);
    }

    private void NextTurnFromTransition()
    {
        if (_turnState == TurnState.Transition)
        {
            SetNextTurn(_previousTurnState == TurnState.Player1 ? TurnState.Player2 : TurnState.Player1);
        }
    }
}
