using System.Collections;
using UnityEngine;

public class InstantiatePickables : MonoBehaviour
{
    [SerializeField] private ParachuteWithWoodBoxController _parachuteWithWoodBoxController;
    private GameManager _gameManager;
    private LevelGenerator _levelGenerator;
    private float _leftSideX, _rightSideX;
    private Vector3 RandomSpawnPosition
    {
        get
        {
            return new Vector3(Random.Range(_leftSideX, _rightSideX), 5, 0);
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
        _leftSideX = _levelGenerator.MapHorizontalStartPoint;
        _rightSideX = _levelGenerator.MapHorizontalEndPoint;

        StartCoroutine(InstantiateCoroutine());
    }

    private IEnumerator InstantiateCoroutine()
    {
        while (true)
        {
            if(FindObjectOfType<ParachuteWithWoodBoxController>() == null)
            {
                ParachuteWithWoodBoxController parachute = Instantiate(_parachuteWithWoodBoxController, RandomSpawnPosition, Quaternion.identity);
            }

            yield return new WaitForSeconds(5);
        }
    }
}
