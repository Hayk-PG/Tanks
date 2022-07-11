using System.Collections;
using UnityEngine;

public class PlayerArtillery : MonoBehaviour
{
    [SerializeField] private ArtilleryBulletController _artilleryPrefab;

    private TankController _tankController;
    private IScore _iScore;
    private PlayerTurn _playerTurn;
    private PropsTabCustomization _propsTabCustomization;
    private RemoteControlArtilleryTarget _remoteControlArtilleryTargets;
    private GameManager _gameManager;
    private CameraMovement _cameraMovement;
    
    private Transform _enemyTransform;



    private void Awake()
    {
        _tankController = Get<TankController>.From(gameObject);
        _iScore = Get<IScore>.From(gameObject);
        _playerTurn = Get<PlayerTurn>.From(gameObject);
        _propsTabCustomization = FindObjectOfType<PropsTabCustomization>();
        _remoteControlArtilleryTargets = FindObjectOfType<RemoteControlArtilleryTarget>();
        _gameManager = FindObjectOfType<GameManager>();
        _cameraMovement = FindObjectOfType<CameraMovement>();
    }

    private void OnEnable()
    {
        _tankController.OnInitialize += OnInitialize;
        _gameManager.OnGameStarted += OnGameStarted;
    }

    private void OnDisable()
    {
        _tankController.OnInitialize -= OnInitialize;
        _gameManager.OnGameStarted -= OnGameStarted;
        _propsTabCustomization.OnArtillery -= OnArtillery;
        _remoteControlArtilleryTargets.OnSet -= OnGiveCoordinates;
    }

    private void OnInitialize()
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
            _cameraMovement.SetCameraTarget(_enemyTransform, 5, 1.5f);
            _remoteControlArtilleryTargets.RemoteControlTargetActivity(true);
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
