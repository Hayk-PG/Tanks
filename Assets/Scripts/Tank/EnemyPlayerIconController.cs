﻿using UnityEngine;

public class EnemyPlayerIconController : MonoBehaviour
{
    private EnemyPlayerIcon _enemyPlayerIcon;
    private PlayerTurn _playerTurn;

    private bool _isIconInitialCoordiantesSet;



    private void Awake()
    {
        _enemyPlayerIcon = FindObjectOfType<EnemyPlayerIcon>();
        _playerTurn = Get<PlayerTurn>.From(gameObject);
    }

    private void OnEnable()
    {
        _playerTurn.OnTurnChangeEventReceived += OnTurnChangeEventReceived;
        _enemyPlayerIcon.OnIconMove += OnIconMove;
    }
   
    private void OnDisable()
    {
        _playerTurn.OnTurnChangeEventReceived -= OnTurnChangeEventReceived;
        _enemyPlayerIcon.OnIconMove -= OnIconMove;
    }

    private void OnTurnChangeEventReceived(TurnState currentTurn, TurnState myTurn)
    {
        if (!_isIconInitialCoordiantesSet)
        {
            Conditions<Transform>.CheckNull(GlobalFunctions.ObjectsOfType<PlayerTurn>.Find(playerturn => playerturn != _playerTurn).transform, null, () => OnEnemyPlayerIcon(myTurn));
        }
    }

    private void OnEnemyPlayerIcon(TurnState myTurn)
    {
        Transform enemyTransform = GlobalFunctions.ObjectsOfType<PlayerTurn>.Find(playerturn => playerturn != _playerTurn).transform;
        _enemyPlayerIcon.SetInitialCoordinates(myTurn == TurnState.Player1, enemyTransform);
        _isIconInitialCoordiantesSet = true;
    }

    private void OnIconMove(System.Action obj)
    {
        obj?.Invoke();
    }
}