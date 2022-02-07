using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class WindSystemController : MonoBehaviour
{
    [SerializeField]
    private int _currentWindForce, _minWindForce, _maxWindForce;
    [SerializeField]
    private int _currentInterval, _minInterval, _maxInterval;

    private IEnumerator _windCoroutine;
    private GameManager _gameManager;

    public int WindForce => _currentWindForce;


    private void Awake()
    {
        _gameManager = GetComponent<GameManager>();
    }

    private void OnEnable()
    {
        _gameManager.OnGameStarted += StartWindCoroutine;
    }
   
    private void OnDisable()
    {
        _gameManager.OnGameStarted -= StartWindCoroutine;
    }

    public void StartWindCoroutine()
    {
        StopWindCoroutine();
        _windCoroutine = WindCoroutine(_gameManager.IsGameRunning);
        StartCoroutine(_windCoroutine);
    }

    public void StopWindCoroutine()
    {
        if (_windCoroutine != null) StopCoroutine(_windCoroutine);
    }

    private IEnumerator WindCoroutine(bool isGameRunning)
    {
        while (isGameRunning)
        {
            _currentWindForce = Random.Range(_minWindForce, _maxWindForce);
            _currentInterval = Random.Range(_minInterval, _maxInterval);

            yield return new WaitForSeconds(_currentInterval);
        }
    }
}
