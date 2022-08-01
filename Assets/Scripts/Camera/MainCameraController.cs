using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    private Camera _camera;
    private Camera _hudCamera;
    private GameManager _gameManager;
    private TurnController _turnController;
    private LevelGenerator _levelGenerator;
    private Transform _player1, _player2;
    private Vector3 _currentVelocity;
    private float _currentVelocityfloat;
    private float _ortographicSize;
    [SerializeField] private float _smoothTime, _maxTime;
    private float _minPosX, _maxPosX, _newPosX;
    private float _yOffset = 2;

    private bool PlayersInitialized
    {
        get => Target1 != null && Target2 != null;
    }
    private float SceneWidth
    {
        get => Mathf.Abs(Target2.position.x) + Mathf.Abs(Target1.position.x) + 1;
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
        _levelGenerator = FindObjectOfType<LevelGenerator>();
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

    private void FixedUpdate()
    {
        if (PlayersInitialized)
        {
            Vector3 targetPosition = new Vector3(Center.x, Center.y + _yOffset, transform.position.z);
            Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, _smoothTime, _maxTime);
            transform.position = ClampPosition(smoothPosition);
            _ortographicSize = Mathf.SmoothDamp(_camera.orthographicSize, DesiredHeight, ref _currentVelocityfloat, _smoothTime, _maxTime);
            _camera.orthographicSize = Mathf.Clamp(_ortographicSize, 1.5f, 5);
            _hudCamera.orthographicSize = _camera.orthographicSize;
        }
    }

    private Vector3 ClampPosition(Vector3 position)
    {
        _minPosX = _levelGenerator.MapHorizontalStartPoint + CameraWidth - 3;
        _maxPosX = _levelGenerator.MapHorizontalEndPoint - CameraWidth + 3;
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

    private void OnTurnChanged(TurnState turnState)
    {
        if (turnState == TurnState.Transition)
            ResetTargets();
    }

    public void ResetTargets()
    {
        Target1 = _player1;
        Target2 = _player2;

        OffsetY(2);
    }

    public void SetTarget(PlayerTurn playerTurn, Transform target, float? yOffset)
    {
        if (Vector3.Distance(Target1.position, Target2.position) >= 5)
        {
            if (playerTurn.MyTurn == TurnState.Player1)
            {
                Target1 = target;
                Target2 = _player2;
            }
            else
            {
                Target1 = _player1;
                Target2 = target;
            }
        }
        else
        {
            Target1 = _player1;
            Target2 = target;
        }

        if (yOffset != null)
            OffsetY(yOffset.Value);
    }
}

