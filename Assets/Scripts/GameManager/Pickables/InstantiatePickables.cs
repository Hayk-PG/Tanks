using Photon.Pun;
using System.Collections;
using UnityEngine;

public class InstantiatePickables : MonoBehaviourPun
{
    [SerializeField]
    private ParachuteWithWoodBoxController _woodBoxControllerPrefab, _instantiatedWoodBoxController;

    private GameManager _gameManager;
    private Transform _player1, _player2;
    private Vector3 _spawnPosition;



    private void Awake()
    {
        _gameManager = Get<GameManager>.From(gameObject);
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

    private void SpawnWoodBox(Vector3 position)
    {
        _instantiatedWoodBoxController = Instantiate(_woodBoxControllerPrefab, position, Quaternion.identity);
    }

    [PunRPC]
    private void SpawnWoodBoxRPC(Vector3 position)
    {
        SpawnWoodBox(position);
    }

    private IEnumerator InstantiateCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(30, 120));

            if (_instantiatedWoodBoxController == null)
            {
                _spawnPosition = new Vector3(Random.Range(_player1.position.x, _player2.position.x), 5, 0);

                if (MyPhotonNetwork.IsOfflineMode)
                {
                    SpawnWoodBox(_spawnPosition);
                }

                if (MyPhotonNetwork.AmPhotonViewOwner(photonView))
                {
                    photonView.RPC("SpawnWoodBoxRPC", RpcTarget.AllViaServer, _spawnPosition);
                }
            }            
        }
    }
}
