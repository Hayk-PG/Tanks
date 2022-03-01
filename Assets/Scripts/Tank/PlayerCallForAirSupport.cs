using UnityEngine;
using System;

public class PlayerCallForAirSupport : MonoBehaviour
{
    [SerializeField]
    private bool _callForAirSupport;
    private bool _isAirSupportCalled;

    private Bomber _bomber;

    private RequestAirSupport _requestAirSupport;
    private PlayerTurn _playerTurn;
    private ShootButton _shootButton;

    private delegate Vector3 Vector();
    private delegate Quaternion Rotation();
    private Vector _bomberPosition;
    private Rotation _bomberRotation;


    private void Awake()
    {
        _requestAirSupport = FindObjectOfType<RequestAirSupport>();
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
        _requestAirSupport.OnRequestAirSupport += OnRequestAirSupport;
        _shootButton.OnClick += OnShootButtonClick;
    }
   
    private void OnDisable()
    {
        _requestAirSupport.OnRequestAirSupport += OnRequestAirSupport;
        _shootButton.OnClick -= OnShootButtonClick;
    }

    private void Update()
    {
        if (_callForAirSupport && _playerTurn.IsMyTurn)
        {
            _requestAirSupport.CallOnRequestAirSupport();
            _isAirSupportCalled = true;
            _callForAirSupport = false;
        }
    }

    private void OnRequestAirSupport(Bomber bomber, Action<RequestAirSupport.InstantiateProperties> Bomber)
    {
        _bomber = bomber;
        Bomber?.Invoke(new RequestAirSupport.InstantiateProperties(_bomberPosition(), _bomberRotation()));
    }

    private void OnShootButtonClick(bool isTrue)
    {
        if (isTrue && _isAirSupportCalled)
        {
            _isAirSupportCalled = false;
            _bomber?.DropBomb();
        }
    }
}
