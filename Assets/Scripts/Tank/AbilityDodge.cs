using System;
using System.Collections;
using UnityEngine;

public class AbilityDodge : MonoBehaviour, IPlayerAbility
{
    private enum AbilitiesOrders { First, Second}

    [SerializeField]
    private AbilitiesOrders _abilitiesOrders;

    [SerializeField] [Space]
    private Rigidbody _rigidbody;

    [SerializeField] [Space]
    private PlayerTurn _playerTurn;

    private BaseBulletController _projectile;

    private IPlayerAbility _iPlayerAbility;

    private MeshRenderer[] _meshes;

    private bool _isActive;
    private bool _isDodged;
    private bool _isOpponentsTurn;

    private Vector3 _positionCurrentTile;
    private Vector3 _positionNextTile;
    private Vector3 _positionLast;

    private int Price { get; set; } = 1300;
    private int Quantity { get; set; } = 0;
    private int UsageFrequency { get; set; } = 3;
    private int Turns { get; set; } = 3;

    private string Title { get; set; } = "Dodge";
    private string Ability { get; set; } = "Avoid next incoming attacks for 3 turn.";

    public event Action onDodge;





    private void Awake() => _iPlayerAbility = this;

    private void Start()
    {
        GameSceneObjectsReferences.DropBoxSelectionPanelPlayerAbilities[(int)_abilitiesOrders].Initialize(_iPlayerAbility, Title, Ability, Price, Quantity, UsageFrequency, Turns);
    }

    private void OnEnable()
    {
        DropBoxSelectionHandler.onItemSelect += OnDropBoxItemSelect;

        GameSceneObjectsReferences.TurnController.OnTurnChanged += OnTurnChanged;
    }

    private void FixedUpdate()
    {
        if (_isActive)
        {
            if (!_isDodged && _isOpponentsTurn && ProjectileDistane() < 5)
                Dodge();
        }
    }

    private void OnDropBoxItemSelect(DropBoxItemType dropBoxItemType, object[] data)
    {
        if(dropBoxItemType == DropBoxItemType.Ability && (IPlayerAbility)data[0] == _iPlayerAbility)
        {
            _isActive = true;

            print($"Dodge is activated!");

            //Extend this
            //Attach script to offline player 
        }
    }

    private void Dodge()
    {
        SetMeshesActive(false);

        transform.position = _positionNextTile; 
        
        _isDodged = true; 
    }

    private void SetMeshesActive(bool isActive)
    {
        if (_meshes == null)
            _meshes = GetComponentsInChildren<MeshRenderer>();

        if (_meshes == null)
            return;

        onDodge?.Invoke();

        GlobalFunctions.Loop<MeshRenderer>.Foreach(_meshes, meshes => { meshes.gameObject.SetActive(isActive); });
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
            _isOpponentsTurn = false;

            StartCoroutine(ResetDodge());
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

    private IEnumerator ResetDodge()
    {
        yield return new WaitForSeconds(0.25f);

        if (_isDodged)
        {
            SetMeshesActive(true);

            if (GameSceneObjectsReferences.TilesData.TilesDict.ContainsKey(_positionCurrentTile - new Vector3(0, 0.5f, 0)))
            {
                transform.position = _positionCurrentTile + new Vector3(0, 0.5f, 0);

                //transform.position = _positionLast;
            }
            
            _isDodged = false;
        }
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
