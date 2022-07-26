using UnityEngine;
using TMPro;

public class Tab_ShootValues : MonoBehaviour
{
    [SerializeField] private TMP_Text _textShootForce, _textCanonAngle;
    private CanvasGroup _canvasGroup;
    private GameManager _gameManager;
    private PlayerHUDShootValues _playerHudShootValues;
    private TankMovement _tankMovement;
    private int _f;


    private void Awake()
    {
        _canvasGroup = Get<CanvasGroup>.From(gameObject);
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void OnEnable()
    {
        _gameManager.OnGameStarted += OnGameStarted;
    }

    private void OnDisable()
    {
        _gameManager.OnGameStarted -= OnGameStarted;

        if(_playerHudShootValues != null)
            _playerHudShootValues.OnPlayerHudShootValues -= OnPlayerHudShootValues;
    }

    private void OnGameStarted()
    {
        TankController playerTank = GlobalFunctions.ObjectsOfType<TankController>.Find(tank => tank.BasePlayer != null);

        if (playerTank != null)
        {
            _playerHudShootValues = playerTank.GetComponentInChildren<PlayerHUDShootValues>();
            _tankMovement = Get<TankMovement>.From(_playerHudShootValues.gameObject);
            _playerHudShootValues.OnPlayerHudShootValues += OnPlayerHudShootValues;
        }            
    }

    private void OnPlayerHudShootValues(string force, string angle)
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, _tankMovement.Direction == 0);

        if (int.TryParse(force, out int result))
        {
            if(result > 0)
                _textShootForce.text = force;

            _textCanonAngle.text = angle;
        }
    }
}
