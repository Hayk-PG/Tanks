using UnityEngine;

public class TankOutline : MonoBehaviour
{
    [SerializeField] private Color[] _outlineColors;
    private GameManager _gameManager;
    private PlayerTurn _playerTurn;
    private MeshRenderer _meshRenderer;


    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _playerTurn = Get<PlayerTurn>.From(gameObject);
    }

    private void OnEnable()
    {
        _gameManager.OnGameStarted += OnGameStarted;
    }

    private void OnDisable()
    {
        _gameManager.OnGameStarted -= OnGameStarted;
    }

    private void OnGameStarted()
    {
        if(_playerTurn != null)
        {
            for (int i = 0; i < _playerTurn.GetComponentsInChildren<MeshRenderer>(true).Length; i++)
            {                
                Outline outline = _playerTurn.GetComponentsInChildren<MeshRenderer>(true)[i].gameObject.AddComponent<Outline>();

                if (outline != null)
                {
                    outline.OutlineMode = Outline.Mode.OutlineVisible;
                    outline.OutlineWidth = 1;
                    outline.OutlineColor = _playerTurn.MyTurn == TurnState.Player1 ? _outlineColors[0] : _outlineColors[1];
                }
            }
        }  
    }
}
