using UnityEngine;

public class TankOutline : MonoBehaviour
{
    [SerializeField] private Color[] _outlineColors;
    private GameManager _gameManager;
    private PlayerTurn _playerTurn;
    private MeshRenderer _meshRenderer;
    private Outline _outline;


    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _playerTurn = Get<PlayerTurn>.From(gameObject);
    }

    private void OnEnable() => _gameManager.OnGameStarted += OnGameStarted;

    private void OnDisable() => _gameManager.OnGameStarted -= OnGameStarted;

    private void OnGameStarted()
    {
        if(_playerTurn != null)
        {
            for (int i = 0; i < _playerTurn.GetComponentsInChildren<MeshRenderer>(true).Length; i++)
            {
                _meshRenderer = _playerTurn.GetComponentsInChildren<MeshRenderer>(true)[i];

                if (_meshRenderer != null)
                {
                    if (Get<Outline>.From(_meshRenderer.gameObject) == null)
                    {
                        _outline = _meshRenderer.gameObject.AddComponent<Outline>();
                        _outline.OutlineMode = Outline.Mode.OutlineVisible;
                        _outline.OutlineWidth = 1;
                        _outline.OutlineColor = _playerTurn.MyTurn == TurnState.Player1 ? _outlineColors[0] : _outlineColors[1];
                    }
                }
            }
        }  
    }
}
