using System.Collections;
using UnityEngine;

public class PlayerArtillery : PlayerDeployProps
{
    [SerializeField] private ArtilleryBulletController _artilleryPrefab;

    private IScore _iScore;
    private RemoteControlArtilleryTarget _remoteControlArtilleryTargets;
    private GameManager _gameManager; 
    private Transform _enemyTransform;


    protected override void Awake()
    {
        base.Awake();

        _iScore = Get<IScore>.From(gameObject);
        _remoteControlArtilleryTargets = FindObjectOfType<RemoteControlArtilleryTarget>();
        _gameManager = FindObjectOfType<GameManager>();
    }

    protected override void Start()
    {
        InitializeRelatedPropsButton(Names.LightMortarSupport);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        _gameManager.OnGameStarted += OnGameStarted;
    }

    protected override void OnDisable()
    {
        _tankController.OnInitialize -= OnInitialize;
        _gameManager.OnGameStarted -= OnGameStarted;
        _propsTabCustomization.OnArtillery -= OnArtillery;
        _remoteControlArtilleryTargets.OnSet -= OnGiveCoordinates;
    }

    protected override void OnInitialize()
    {
        _propsTabCustomization.OnArtillery += OnArtillery;
        _remoteControlArtilleryTargets.OnSet += OnGiveCoordinates;
    }

    private void OnGameStarted()
    {
        _enemyTransform = GlobalFunctions.ObjectsOfType<PlayerTurn>.Find(playerTurn => playerTurn != _playerTurn).transform;
    }

    private void OnArtillery()
    {
        if (_playerTurn.IsMyTurn && _enemyTransform != null)
        {
            _remoteControlArtilleryTargets.RemoteControlTargetActivity(true);
            _propsTabCustomization.OnSupportOrPropsChanged?.Invoke(_relatedPropsTypeButton);
        }         
    }

    private void OnGiveCoordinates(Vector3 coordinate)
    {
        StartCoroutine(InstantiateArtilleryShells(coordinate));
    }

    private IEnumerator InstantiateArtilleryShells(Vector3 coordinate)
    {
        for (int i = 0; i < 5; i++)
        {
            float randomX = i <= 3 ? Random.Range(-2, 2) : 0;
            ArtilleryBulletController artillery = Instantiate(_artilleryPrefab, new Vector3(coordinate.x + randomX, coordinate.y + 10, coordinate.z), Quaternion.identity);
            artillery.OwnerScore = _iScore;
            artillery.IsLastShellOfBarrage = i < 4 ? false : true;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
