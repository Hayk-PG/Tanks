using System.Collections.Generic;
using System;
using Photon.Pun;
using System.Collections;
using UnityEngine;

public enum TurnState { None, Transition, Player1, Player2, Other}

public class TurnController : MonoBehaviourPun
{
    public TurnState _turnState;
    public TurnState _previousTurnState;

    private GameManager _gameManager;
    private CameraMovement _cameraMovement;

    private List<PlayerTurn> _playersTurn;

    public Action<TurnState,CameraMovement> OnTurnChanged { get; set; }

    /// <summary>
    /// 0: PlayerOne 1:PlayerTwo
    /// </summary>
    public static List<PlayerTurn> Players { get; set; } = new List<PlayerTurn>();
    public event Action<List<PlayerTurn>> OnPlayers;
    

    private void Awake()
    {
        _gameManager = Get<GameManager>.From(gameObject);
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

    private void SetPlayersTurnOnGameStart()
    {
        SetNextTurn(TurnState.Player1);
    }

    public void SetNextTurn(TurnState turnState)
    {
        if (!MyPhotonNetwork.IsOfflineMode && photonView.IsMine)
        {
            photonView.RPC("NextTurn", RpcTarget.AllViaServer,(int)_turnState, (int)turnState);
            StartCoroutine(NextTurnFromTransitionCoroutine((int)_turnState));
        }
            
        else if (MyPhotonNetwork.IsOfflineMode)
        {
            NextTurn((int)_turnState, (int)turnState);
            StartCoroutine(NextTurnFromTransitionCoroutine((int)_turnState));
        }
    }

    [PunRPC]
    private void NextTurn(int currentTurnState, int nextTurnState)
    {
        if ((TurnState)currentTurnState == TurnState.Player1 || (TurnState)currentTurnState == TurnState.Player2)
            _previousTurnState = (TurnState)currentTurnState;

        _turnState = (TurnState)nextTurnState;
        OnTurnChanged?.Invoke(_turnState, _cameraMovement);

        print("b");
    }
    
    private IEnumerator NextTurnFromTransitionCoroutine(int currentTurnState)
    {
        yield return new WaitForSeconds(2);

        if (!MyPhotonNetwork.IsOfflineMode)
            photonView.RPC("NextTurnFromTransition", RpcTarget.AllViaServer, currentTurnState);
        else
            NextTurnFromTransition(currentTurnState);

    }

    [PunRPC]
    private void NextTurnFromTransition(int currentTurnState)
    {
        if ((TurnState)currentTurnState == TurnState.Transition)
        {
            SetNextTurn(_previousTurnState == TurnState.Player1 ? TurnState.Player2 : TurnState.Player1);
        }

        print("a");
    }
}
