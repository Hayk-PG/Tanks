using System;
using System.Collections;
using UnityEngine;

public class AbilityDodge : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _rigidbody;

    [SerializeField] [Space]
    private PlayerTurn _playerTurn;

    private BaseBulletController _projectile;

    [SerializeField] [Space]
    private bool _isActive;
    private bool _isDodged;
    private bool _isOpponentsTurn;

    private Vector3 _positionCurrentTile;
    private Vector3 _positionNextTile;
    private Vector3 _positionLast;





    private void OnEnable() => GameSceneObjectsReferences.TurnController.OnTurnChanged += OnTurnChanged;

    private void FixedUpdate()
    {
        if (_isActive)
        {
            if (!_isDodged && _isOpponentsTurn && ProjectileDistane() < 5)
            {
                transform.position = _positionNextTile;

                _isDodged = true;

                //print($"Dodged: ");
            }
        }
    }

    private void OnTurnChanged(TurnState turnState)
    {
        if (_isActive)
        {
            OnMyTurn();

            OnOpponentTurn(turnState);

            OnOtherTurn(turnState);
        }

    }

    private void OnMyTurn()
    {
        if (_playerTurn.IsMyTurn)
        {
            StartCoroutine(ResetTransformPosition());

            SetConditionsFalse();
        }
    }

    private void OnOpponentTurn(TurnState turnState)
    {
        if (OpponentTankTurn(turnState))
        {
            StartCoroutine(RunIterations());

            GetLastPosition();

            _isOpponentsTurn = true;
        }
    }

    private void OnOtherTurn(TurnState turnState)
    {
        if (turnState == TurnState.Other)
            GetProjectile();
    }

    private IEnumerator ResetTransformPosition()
    {
        yield return new WaitForSeconds(0.25f);

        if (_isDodged && GameSceneObjectsReferences.TilesData.TilesDict.ContainsKey(_positionCurrentTile) && transform.position != _positionLast)
            transform.position = _positionLast;
    }

    private void SetConditionsFalse()
    {
        _isDodged = false;

        _isOpponentsTurn = false;
    }

    private void GetLastPosition() => _positionLast = transform.position;

    private void GetProjectile() => _projectile = FindObjectOfType<BaseBulletController>();

    private IEnumerator RunIterations()
    {
        yield return StartCoroutine(GetTilesPositions(0f, 0.5f, false, GetCurrentTilePosition));
        yield return StartCoroutine(GetTilesPositions(1, 5.5f, true, GetNextTilePosition));
    }

    private IEnumerator GetTilesPositions(float minDistance, float maxDistance, bool excludeTiles, Action<Vector3> result = null)
    {
        foreach (var tileDict in GameSceneObjectsReferences.TilesData.TilesDict)
        {
            if (Vector3.Distance(transform.position, tileDict.Key) >= minDistance && Vector3.Distance(transform.position, tileDict.Key) <= maxDistance)
            {
                bool isFound = false;

                if (excludeTiles)
                {
                    isFound = tileDict.Value.name != Names.B && tileDict.Value.name != Names.BL && tileDict.Value.name != Names.BR &&
                              tileDict.Value.name != Names.L && tileDict.Value.name != Names.M && tileDict.Value.name != Names.R &&
                              tileDict.Value.name != Names.RL;
                }
                else
                    isFound = true;

                yield return null;

                if (isFound)
                {
                    result?.Invoke(tileDict.Key);

                    print($"{excludeTiles}: {tileDict.Value.name}");

                    yield break;
                }
            }
        }
    }

    private void GetCurrentTilePosition(Vector3 positionCurrentTile) => _positionCurrentTile = positionCurrentTile;

    private void GetNextTilePosition(Vector3 positionNextTile) => _positionNextTile = positionNextTile + Vector3.up;

    private bool OpponentTankTurn(TurnState turnState)
    {
        bool isOneOfPlayersTurn = turnState == TurnState.Player1 || turnState == TurnState.Player2;

        return isOneOfPlayersTurn && turnState != _playerTurn.MyTurn;
    }
     
    private float ProjectileDistane()
    {
        return _projectile != null ? Vector3.Distance(_rigidbody.position, _projectile.RigidBody.position) : 100;
    }
}
