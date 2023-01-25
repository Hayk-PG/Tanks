using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    private Camera _camera;
    private Camera _hudCamera;
    private GameManager _gameManager;
    private TurnController _turnController;
    private MapPoints _mapPoints;
    private Transform _player1, _player2;
    
    [SerializeField] private float _smoothTime, _maxTime;
    private float _currentVelocityfloat;
    private float _ortographicSize;  
    private float _minPosX, _maxPosX, _newPosX;
    private float _yOffset = 2;
    private float _desiredHeightOffset = 0;

    private Vector3 _currentVelocity;
    private Vector3 _halfLength = new Vector3(0.5f, 0, 0);

    private bool PlayersInitialized
    {
        get => Target1 != null && Target2 != null;
    }
    private float SceneWidth
    {
        get => Vector3.Distance(Target1.position - _halfLength, Target2.position + _halfLength);
    }
    private float UnitsPerPixel
    {
        get => SceneWidth / Screen.width;
    }
    private float DesiredHeight
    {
        get => 0.5f * UnitsPerPixel * Screen.height;
    }
    private Vector3 Center
    {
        get => Vector3.Lerp(Target1.position, Target2.position, 0.5f);
    }
    private Transform Target1 { get; set; }
    private Transform Target2 { get; set; }
    private float CameraWidth => _camera.orthographicSize * _camera.aspect;


    private void Awake()
    {
        _camera = Get<Camera>.From(gameObject);
        _hudCamera = Get<Camera>.From(transform.Find("HUDCamera").gameObject);
        _gameManager = FindObjectOfType<GameManager>();
        _turnController = FindObjectOfType<TurnController>();
        _mapPoints = FindObjectOfType<MapPoints>();
    }

    private void OnEnable()
    {
        _gameManager.OnGameStarted += OnGameStarted;
        _turnController.OnTurnChanged += OnTurnChanged;
    }

    private void OnDisable()
    {
        _gameManager.OnGameStarted -= OnGameStarted;
        _turnController.OnTurnChanged -= OnTurnChanged;
    }

    private void Update()
    {
        if (PlayersInitialized)
        {
            Vector3 targetPosition = new Vector3(Center.x, Center.y + _yOffset, transform.position.z);
            Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, _smoothTime, _maxTime);
            transform.position = smoothPosition;
            _ortographicSize = Mathf.SmoothDamp(_camera.orthographicSize, DesiredHeight + _desiredHeightOffset, ref _currentVelocityfloat, _smoothTime, _maxTime);
            _camera.orthographicSize = Mathf.Clamp(_ortographicSize, 2, 10);
            _hudCamera.orthographicSize = _camera.orthographicSize;
        }
    }

    private Vector3 ClampPosition(Vector3 position)
    {
        _minPosX = _mapPoints.HorizontalMin + CameraWidth - 3;
        _maxPosX = _mapPoints.HorizontalMax - CameraWidth + 3;
        _newPosX = Mathf.Clamp(position.x, _minPosX, _maxPosX);
        return new Vector3(_newPosX, position.y, position.z);
    }

    private void OnGameStarted()
    {
        _player1 = GlobalFunctions.ObjectsOfType<PlayerTurn>.Find(playerturn => playerturn.MyTurn == TurnState.Player1).transform;
        _player2 = GlobalFunctions.ObjectsOfType<PlayerTurn>.Find(playerturn => playerturn.MyTurn == TurnState.Player2).transform;

        ResetTargets();
    }

    private void OffsetY(float value)
    {
        _yOffset = value;
    }

    private void DesiredHeightOffset(float value)
    {
        _desiredHeightOffset = value;
    }

    private void SetTargets(Transform target1, Transform target2)
    {
        Target1 = target1;
        Target2 = target2;
    }

    public void ResetTargets()
    {
        SetTargets(_player1, _player2);
        OffsetY(2);
        DesiredHeightOffset(0);
    }

    private void OnTurnChanged(TurnState turnState)
    {
        if (turnState == TurnState.Transition)
            ResetTargets();
    }

    public void CameraOffset(PlayerTurn playerTurn, Transform target, float? yOffset, float? desiredHeightOffset)
    {
        //bool isDistanceAcceptable = Vector3.Distance(Target1 != null ? Target1.position: _player1.position, Target1 != null ? Target2.position : _player2.position) >= 5;
        bool isPlayerOnesTurn = playerTurn?.MyTurn == TurnState.Player1;

        if (playerTurn != null)
        {
            if (isPlayerOnesTurn)
            {
                SetTargets(target, _player2);
                OffsetY(Mathf.Lerp(yOffset.HasValue ? yOffset.Value : 2, Target2.position.y, 0.5f));
            }
            if (!isPlayerOnesTurn)
            {
                SetTargets(_player1, target);
                OffsetY(Mathf.Lerp(yOffset.HasValue ? yOffset.Value : 2, Target1.position.y, 0.5f));
            }
            //if (!isDistanceAcceptable)
            //    SetTargets(_player1, target);
        }
        else
            SetTargets(_player1, _player2);

        DesiredHeightOffset(desiredHeightOffset.HasValue ? desiredHeightOffset.Value : 0);
    }
}

