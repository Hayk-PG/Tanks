using UnityEngine;

public class TerritoryOccupiedIndicator : MonoBehaviour
{
    [SerializeField]
    private MapPoints _mapPoints;

    [SerializeField] [Space]
    private GameManager _gameManager;

    [SerializeField] [Space]
    private RectTransform _player1, _player2;

    private Transform _tank1Transform;
    private Transform _tank2Transform;

    private float _zeroValue = 1589;
    private float _player1Value;
    private float _player2Value;

    public int Player1Percentage { get; private set; }
    public int Player2Percentage { get; private set; }


 


    private void OnEnable() => _gameManager.OnGameStarted += OnGameStarted;

    private void OnDisable() => _gameManager.OnGameStarted -= OnGameStarted;

    private void FixedUpdate()
    {
        if (_tank1Transform != null)
        {
            _player1Value = Mathf.InverseLerp(_mapPoints.HorizontalMax, _mapPoints.HorizontalMin, _tank1Transform.position.x);

            _player1.offsetMax = new Vector2(-(_player1Value * _zeroValue), _player1.offsetMax.y);

            Player1Percentage = Mathf.FloorToInt((_zeroValue - Mathf.Abs(_player1.offsetMax.x)) / _zeroValue * 100);
        }

        if (_tank2Transform != null)
        {
            _player2Value = Mathf.InverseLerp(_mapPoints.HorizontalMin, _mapPoints.HorizontalMax, _tank2Transform.position.x);

            _player2.offsetMin = new Vector2((_player2Value * _zeroValue), _player2.offsetMin.y);

            Player2Percentage = Mathf.FloorToInt((_zeroValue - Mathf.Abs(_player2.offsetMin.x)) / _zeroValue * 100);
        }
    }

    private void OnGameStarted()
    {
        _tank1Transform = _gameManager.Tank1?.transform;
        _tank2Transform = _gameManager.Tank2?.transform;
    }
}
