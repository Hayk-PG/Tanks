using UnityEngine;
using System;

public class PlayerRequestAirSupport : MonoBehaviour
{
    [SerializeField]
    private bool _callForAirSupport;
    private bool _isAirSupportRequested;

    private Bomber _bomber;

    private AirSupport _airSupport;
    private PlayerTurn _playerTurn;
    private ShootButton _shootButton;

    private delegate Vector3 Vector();
    private delegate Quaternion Rotation();
    private Vector _bomberPosition;
    private Rotation _bomberRotation;


    private void Awake()
    {
        _airSupport = FindObjectOfType<AirSupport>();
        _playerTurn = Get<PlayerTurn>.From(gameObject);
        _shootButton = FindObjectOfType<ShootButton>();
    }

    private void Start()
    {
        _bomberPosition = delegate
        {
            return _playerTurn.MyTurn == TurnState.Player1 ? new Vector3(15, 3, 0) : new Vector3(-15, 3, 0);
        };

        _bomberRotation = delegate
        {
            return _playerTurn.MyTurn == TurnState.Player1 ? Quaternion.Euler(-90, -90, 0) : Quaternion.Euler(-90, 90, 0);
        };
    }

    private void OnEnable()
    {
        _airSupport.OnRequestAirSupport += OnRequestAirSupport;
        _shootButton.OnClick += OnShootButtonClick;
    }
   
    private void OnDisable()
    {
        _airSupport.OnRequestAirSupport += OnRequestAirSupport;
        _shootButton.OnClick -= OnShootButtonClick;
    }

    private void Update()
    {
        if (_callForAirSupport && _playerTurn.IsMyTurn)
        {
            _airSupport.CallOnRequestAirSupport();
            _isAirSupportRequested = true;
            _callForAirSupport = false;
        }
    }

    private void OnRequestAirSupport(Bomber bomber, Action<AirSupport.InstantiateProperties> ActivateBomber)
    {
        _bomber = bomber;
        ActivateBomber?.Invoke(new AirSupport.InstantiateProperties(_bomberPosition(), _bomberRotation()));
    }

    private void OnShootButtonClick(bool isTrue)
    {
        if (isTrue && _isAirSupportRequested)
        {
            _isAirSupportRequested = false;
            _bomber?.DropBomb();
        }
    }
}
