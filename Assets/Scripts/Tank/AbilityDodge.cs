using System;
using System.Collections;
using UnityEngine;

public class AbilityDodge : BaseAbility, IPlayerAbility
{
    [SerializeField] [Space]
    private Rigidbody _rigidbody;

    [SerializeField] [Space]
    private Transform _shootPoint;

    private BaseBulletController _projectile;

    private MeshRenderer[] _meshes;

    private bool _isDodged;
    private bool _isOpponentsTurn;

    private Vector3 _shootPointDefaultPosition;
    private Vector3 _positionCurrentTile;
    private Vector3 _positionNextTile;

    protected override string Title => "Dodge";
    protected override string Ability => $"Avoid incoming attacks for {Turns} turn.";






    protected override void Awake()
    {
        base.Awake();

        _shootPointDefaultPosition = _shootPoint.localPosition;
    }

    private void FixedUpdate() => Conditions<bool>.Compare(IsAbilityActive, () => UseAbility(), null);

    private void UseAbility(object[] data = null)
    {
        bool canUseDodgeAbility = !_isDodged && _isOpponentsTurn && ProjectileDistane() < 2.5f;

        if (canUseDodgeAbility)
        {
            SetMeshesActive(false);

            SetShootPointLocalPosition(true);

            transform.position = _positionNextTile;

            _isDodged = true;
        }
    }

    protected override void OnTurnChanged(TurnState turnState)
    {
        if (IsAbilityActive)
        {
            OnMyTurn();

            OnOpponentTurn(turnState);

            OnOtherTurn(turnState);
        }
    }

    private void SetMeshesActive(bool isActive)
    {
        if (_meshes == null)
            _meshes = GetComponentsInChildren<MeshRenderer>();

        if (_meshes == null)
            return;

        RaiseAbilityEvent();

        GlobalFunctions.Loop<MeshRenderer>.Foreach(_meshes, meshes => { meshes.gameObject.SetActive(isActive); });
    }

    private void SetShootPointLocalPosition(bool isAbilityActive) => _shootPoint.localPosition = isAbilityActive ? new Vector3(0, 1000, 0) : _shootPointDefaultPosition;

    private void OnMyTurn()
    {
        if (_playerTurn.IsMyTurn)
        {
            _isOpponentsTurn = false;

            StartCoroutine(ResetDodge());
        }
    }

    private void OnOpponentTurn(TurnState turnState)
    {
        if (OpponentTankTurn(turnState))
        {
            StartCoroutine(RunIterations());

            _isOpponentsTurn = true;
        }
    }

    private void OnOtherTurn(TurnState turnState)
    {
        if (turnState == TurnState.Other)
            GetProjectile();
    }

    private IEnumerator ResetDodge()
    {
        yield return new WaitForSeconds(0.25f);

        if (_isDodged)
        {
            if (GameSceneObjectsReferences.TilesData.TilesDict.ContainsKey(_positionCurrentTile - new Vector3(0, 0.5f, 0)))
                transform.position = _positionCurrentTile + new Vector3(0, 0.5f, 0);

            SetMeshesActive(true);

            SetShootPointLocalPosition(false);

            DeactivateAbilityAfterLimit();

            _isDodged = false;
        }
    }

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
