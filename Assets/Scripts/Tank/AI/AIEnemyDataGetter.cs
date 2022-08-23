using System.Collections;
using UnityEngine;

public class AIEnemyDataGetter : MonoBehaviour
{
    private GameManager _gameManager;

    public Transform Enemy { get; private set; }
    public float Distance { get; private set; }

    


    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
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
        Enemy = GlobalFunctions.ObjectsOfType<TankController>.Find(tank => tank.BasePlayer != null).transform;

        StartCoroutine(DistanceCalculator());
    }

    private IEnumerator DistanceCalculator()
    {
        while (Enemy != null)
        {
            Distance = Vector3.Distance(transform.position, Enemy.position);
            yield return new WaitForSeconds(1);
        }
    }
}
