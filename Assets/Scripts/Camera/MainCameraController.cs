using UnityEngine;

public class MainCameraController : MonoBehaviour, IEndGame
{
    public enum CameraUser { None, RemoteControl}

    private CameraUser _cameraUser;

    [SerializeField]
    private Camera _camera, _hudCamera, _noPPCamera;

    [SerializeField][Space] 
    private Rigidbody _rb;

    [SerializeField][Space] 
    private GameManager _gameManager;

    [SerializeField][Space] 
    private TurnController _turnController;

    [SerializeField][Space] 
    private MapPoints _mapPoints;
    
    private Rigidbody _player1, _player2;

    [SerializeField] [Space]
    private float _smoothTime, _maxTime;
    private float _currentVelocityfloat;
    private float _ortographicSize;  
    private float _minPosX, _maxPosX, _newPosX;
    private float _yOffset = 2;
    private float _desiredHeightOffset = 0;

    private bool _isLocked;

    private Vector3 _currentVelocity;
    private Vector3 _halfLength = new Vector3(0.5f, 0, 0);

    private Rigidbody Target1 { get; set; }
    private Rigidbody Target2 { get; set; }
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

    private System.Func<Vector3> Point1 { get; set; }
    private System.Func<Vector3> Point2 { get; set; }

    private Vector3 Center
    {
        get => Vector3.Lerp(Point1(), Point2(), 0.5f);
    }
    
    private float CameraWidth => _camera.orthographicSize * _camera.aspect;



    private void Start() => ControlCameraPoints();

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

    private void FixedUpdate() => ControlMovement();

    private void LateUpdate() => ControlOrtographicSize();

    private void ControlCameraPoints()
    {
        Point1 = delegate { return Target1?.position ?? Vector3.zero; };

        Point2 = delegate { return Target2?.position ?? Vector3.zero; };
    }

    private void ControlMovement()
    {
        if (PlayersInitialized)
        {
            Vector3 targetPosition = new Vector3(Center.x, Center.y + _yOffset, _rb.position.z);

            Vector3 smoothPosition = Vector3.SmoothDamp(_rb.position, targetPosition, ref _currentVelocity, _smoothTime, _maxTime);

            _rb.MovePosition(smoothPosition);
        }
    }

    private void ControlOrtographicSize()
    {
        if (PlayersInitialized)
        {
            _ortographicSize = Mathf.SmoothDamp(_camera.orthographicSize, DesiredHeight + _desiredHeightOffset, ref _currentVelocityfloat, _smoothTime, _maxTime);

            _camera.orthographicSize = Mathf.Clamp(_ortographicSize, 2, 10);

            _hudCamera.orthographicSize = _camera.orthographicSize;

            _noPPCamera.orthographicSize = _camera.orthographicSize;
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
        PlayerTurn p1 = GlobalFunctions.ObjectsOfType<PlayerTurn>.Find(playerturn => playerturn.MyTurn == TurnState.Player1);
        PlayerTurn p2 = GlobalFunctions.ObjectsOfType<PlayerTurn>.Find(playerturn => playerturn.MyTurn == TurnState.Player2);

        _player1 = Get<Rigidbody>.From(p1.gameObject);
        _player2 = Get<Rigidbody>.From(p2.gameObject);

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

    private void SetTargets(Rigidbody target1, Rigidbody target2)
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

    public void CameraOffset(PlayerTurn playerTurn, Rigidbody target, float? yOffset, float? desiredHeightOffset, CameraUser cameraUser = CameraUser.None, bool isLocked = false)
    {
        if (_cameraUser == CameraUser.None || _cameraUser == cameraUser)
            _isLocked = isLocked;

        _cameraUser = _isLocked ? cameraUser : CameraUser.None;

        if (_isLocked)
            return;

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
        }

        else
            SetTargets(_player1, _player2);

        DesiredHeightOffset(desiredHeightOffset.HasValue ? desiredHeightOffset.Value : 0);
    }

    public void OnGameEnd(object[] data = null)
    {
        print($"{Point1()} / {Point2()}");
    }
}

