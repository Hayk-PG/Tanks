using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;

public class WindSystemController : MonoBehaviourPun
{
    private IEnumerator _windCoroutine;
    private GameManager _gameManager;

    [SerializeField] private int _minWindForce, _maxWindForce;
    [SerializeField] private int _minInterval, _maxInterval;

    public int CurrentWindForce { get; set; }
    public int CurrentInternval { get; set; }
    public int WindForce => CurrentWindForce;

    public Action<int> OnWindForce { get; set; }


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
        if (MyPhotonNetwork.IsOfflineMode && Data.Manager.IsWindOn || !MyPhotonNetwork.IsOfflineMode && (bool)MyPhotonNetwork.CurrentRoom.CustomProperties[Keys.MapWind])
        {
            EnableWind();
        }
    }

    public void EnableWind()
    {
        print("Wind is on");
        StopWindCoroutine();
        _windCoroutine = WindCoroutine(!_gameManager.IsGameEnded);
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
            AssignWindValues();          
            yield return new WaitForSeconds(CurrentInternval);
        }
    }

    private void AssignWindValues()
    {
        if (!MyPhotonNetwork.IsOfflineMode && MyPhotonNetwork.AmPhotonViewOwner(photonView))
        {
            WindValues();
            photonView.RPC("ShareWindForceValue", RpcTarget.AllViaServer, CurrentWindForce);
        }
            
        if (MyPhotonNetwork.IsOfflineMode)
        {
            WindValues();
            ShareWindForceValue(CurrentWindForce);
        }           
    }

    private void WindValues()
    {
        CurrentWindForce = UnityEngine.Random.Range(_minWindForce, _maxWindForce);
        CurrentInternval = UnityEngine.Random.Range(_minInterval, _maxInterval);
    }

    [PunRPC]
    private void ShareWindForceValue(int currentWindForce)
    {
        OnWindForce?.Invoke(currentWindForce);
    }
}
