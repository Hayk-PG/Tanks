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

    public Action<TurnState,CameraMovement> OnTurnChanged { get; set; }
  

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
        if(!MyPhotonNetwork.IsOfflineMode && MyPhotonNetwork.AmPhotonViewOwner(photonView))
        {
            SetPreviousTurnState();
            SetCurrentTurnState(turnState);
            Invoke("NextTurnFromTransition", 2);
            photonView.RPC("OnTurnChangedRPC", RpcTarget.AllViaServer, (int)_turnState);
        }
        if (MyPhotonNetwork.IsOfflineMode)
        {
            SetPreviousTurnState();
            SetCurrentTurnState(turnState);
            Invoke("NextTurnFromTransition", 2);
            OnTurnChanged?.Invoke(_turnState, _cameraMovement);
        }
    }

    private void SetPreviousTurnState()
    {
        if (_turnState == TurnState.Player1 || _turnState == TurnState.Player2)
            _previousTurnState = _turnState;
    }

    private void SetCurrentTurnState(TurnState turnState)
    {
        _turnState = turnState;
    }

    private void NextTurnFromTransition()
    {
        if (_turnState == TurnState.Transition)
            SetNextTurn(_previousTurnState == TurnState.Player1 ? TurnState.Player2 : TurnState.Player1);
    }

    [PunRPC]
    private void OnTurnChangedRPC(int currentTurnState)
    {
        OnTurnChanged?.Invoke((TurnState)currentTurnState, _cameraMovement);
        print("OnTurnChanged" + "/" + currentTurnState);
    }
}
