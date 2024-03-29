﻿using Photon.Pun;
using UnityEngine;

public class BasePlayerTankSpawner<T> : MonoBehaviourPun
{
    protected Transform _spawnPointForPlayer1;
    protected Transform _spawnPointForPlayer2;
    protected T _tankController;

    protected GameManager _gameManager;


    protected virtual void Awake()
    {
        _spawnPointForPlayer1 = GameObject.Find("1PlayerStartPoint").transform;
        _spawnPointForPlayer2 = GameObject.Find("2PlayerStartPoint").transform;
        _tankController = Get<T>.From(gameObject);
        _gameManager = FindObjectOfType<GameManager>();
    }

    public virtual void SpawnTanks(int tankIndex, int spawnPointIndex)
    {
        Vector3 position = spawnPointIndex == 0 ? _spawnPointForPlayer1.position : _spawnPointForPlayer2.position;
        Quaternion rotation = spawnPointIndex == 0 ? _spawnPointForPlayer1.rotation : _spawnPointForPlayer2.rotation;
        TankController tank = Instantiate(Data.Manager.AvailableTanks[tankIndex]._tank, position, rotation);
        tank.name = spawnPointIndex == 0 ? Names.Tank_FirstPlayer : Names.Tank_SecondPlayer;

        CacheSpawnedTank(tank);
        SetSpawnedTanksTurn(spawnPointIndex, tank);
        InitializeGameManagerTankController(spawnPointIndex, tank);
    }

    protected virtual void CacheSpawnedTank(TankController tank)
    {
        
    }

    protected virtual void InitializeGameManagerTankController(int playerIndex, TankController tank)
    {
        if (playerIndex == 0) _gameManager.Tank1 = tank;
        if (playerIndex == 1) _gameManager.Tank2 = tank;
    }

    protected virtual void SetSpawnedTanksTurn(int spawnPointIndex, TankController tank)
    {
        PlayerTurn playerTurn = tank?.GetComponent<PlayerTurn>();
        if(playerTurn != null) playerTurn.MyTurn = spawnPointIndex == 0 ? TurnState.Player1 : TurnState.Player2;
    }
}
