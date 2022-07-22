using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    private Camera _camera;
    private Camera _hudCamera;
    private GameManager _gameManager;
    private TurnController _turnController;
    private Transform _player1, _player2;
    private Vector3 _currentVelocity;
    private float _currentVelocityfloat;
    [SerializeField] private float _smoothTime, _maxTime;
    
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



    private void Awake()
    {
        _camera = Get<Camera>.From(gameObject);
        _hudCamera = Get<Camera>.From(transform.Find("HUDCamera").gameObject);
        _gameManager = FindObjectOfType<GameManager>();
        _turnController = FindObjectOfType<TurnController>();
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
            Vector3 targetPosition = new Vector3(Center.x, Center.y + 2, transform.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, _smoothTime, _maxTime);
            _camera.orthographicSize = Mathf.SmoothDamp(_camera.orthographicSize, DesiredHeight, ref _currentVelocityfloat, _smoothTime, _maxTime);
            _hudCamera.orthographicSize = _camera.orthographicSize;
        }
    }

    private void OnGameStarted()
    {
        _player1 = GlobalFunctions.ObjectsOfType<PlayerTurn>.Find(playerturn => playerturn.MyTurn == TurnState.Player1).transform;
        _player2 = GlobalFunctions.ObjectsOfType<PlayerTurn>.Find(playerturn => playerturn.MyTurn == TurnState.Player2).transform;

        ResetTargets();
    }

    private void OnTurnChanged(TurnState turnState)
    {
        if (turnState == TurnState.Transition)
            ResetTargets();
    }

    private void ResetTargets()
    {
        Target1 = _player1;
        Target2 = _player2;
    }

    public void SetTarget(PlayerTurn playerTurn, Transform target)
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
    }
}

