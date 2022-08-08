using System.Collections;
using UnityEngine;

public class InstantiatePickables : MonoBehaviour
{
    [SerializeField] private ParachuteWithWoodBoxController _parachuteWithWoodBoxController;
    private GameManager _gameManager;
    private LevelGenerator _levelGenerator;
    private Transform _player1, _player2;
    private float _leftSideX, _rightSideX;
    private Vector3 RandomSpawnPosition
    {
        get
        {
            return new Vector3(Random.Range(_player1.position.x, _player2.position.x), 5, 0);
        }
    }
    private bool IsThereOtherActiveParachute
    {
        get => FindObjectOfType<ParachuteWithWoodBoxController>();
    }
    private float RandomTime
    {
        get
        {
            return Random.Range(30, 120);
        }
    }
    private int RandomContent
    {
        get
        {
            return Random.Range(0, 3);
        }
    }


    private void Awake()
    {
        _gameManager = Get<GameManager>.From(gameObject);
        _levelGenerator = FindObjectOfType<LevelGenerator>();
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
        _player1 = GlobalFunctions.ObjectsOfType<PlayerTurn>.Find(turn => turn.MyTurn == TurnState.Player1).transform;
        _player2 = GlobalFunctions.ObjectsOfType<PlayerTurn>.Find(turn => turn.MyTurn == TurnState.Player2).transform;

        StartCoroutine(InstantiateCoroutine());
    }

    private IEnumerator InstantiateCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(RandomTime);

            if (_player1 != null && _player2 != null && !IsThereOtherActiveParachute)
            {
                ParachuteWithWoodBoxController parachute = Instantiate(_parachuteWithWoodBoxController, RandomSpawnPosition, Quaternion.identity);
                parachute.RandomContent = RandomContent;
            }
        }
    }
}
