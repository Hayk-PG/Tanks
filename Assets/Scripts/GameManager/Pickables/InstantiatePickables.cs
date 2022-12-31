using Photon.Pun;
using System.Collections;
using UnityEngine;

public class InstantiatePickables : MonoBehaviourPun
{
    [SerializeField] private ParachuteWithWoodBoxController _woodBoxControllerPrefab;
    private ParachuteWithWoodBoxController _parachuteWithWoodBoxController;

    private GameManager _gameManager;
    private TilesData _tilesData;
    private Transform _player1, _player2;



    private void Awake()
    {
        _gameManager = Get<GameManager>.From(gameObject);
        _tilesData = FindObjectOfType<TilesData>();
    }

    private void OnEnable() => _gameManager.OnGameStarted += OnGameStarted;

    private void OnDisable() => _gameManager.OnGameStarted -= OnGameStarted;

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
            yield return new WaitForSeconds(Random.Range(30, 90));

            if (FindObjectOfType<ParachuteWithWoodBoxController>() == null)
            {
                if (_player1 != null && _player2 != null)
                {
                    if (MyPhotonNetwork.IsOfflineMode)
                        SpawnWoodBox(WoodBoxSpawnPosition());

                    if (MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer))
                        photonView.RPC("SpawnWoodBoxRPC", RpcTarget.AllViaServer, WoodBoxSpawnPosition());
                }
            }            
        }
    }

    private Vector3 WoodBoxSpawnPosition()
    {
        Vector3 tempPosition = new Vector3(Random.Range(_player1.position.x, _player2.position.x), 5, 0);
        Vector3 tilePosition = tempPosition;

        foreach (var tileDict in _tilesData.TilesDict)
        {
            if(tileDict.Key.x >= tempPosition.x - 1 && tileDict.Key.x <= tempPosition.x + 1)
            {
                tilePosition.x = tileDict.Key.x;
                break;
            } 
        }

        return tilePosition;
    }

    private void SpawnWoodBox(Vector3 position)
    {
        _parachuteWithWoodBoxController = Instantiate(_woodBoxControllerPrefab, position, Quaternion.identity);
    }

    [PunRPC]
    private void SpawnWoodBoxRPC(Vector3 position)
    {
        SpawnWoodBox(position);
    }
}
