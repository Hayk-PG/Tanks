using Photon.Pun;
using System.Collections;
using UnityEngine;

public class InstantiatePickables : MonoBehaviourPun
{
    [SerializeField] 
    private ParachuteWithWoodBoxController _woodBoxControllerPrefab;

    private ParachuteWithWoodBoxController _parachuteWithWoodBoxController;

    private Transform _player1, _player2;




    private void OnEnable() => GameSceneObjectsReferences.GameManager.OnGameStarted += OnGameStarted;

    private void OnDisable() => GameSceneObjectsReferences.GameManager.OnGameStarted -= OnGameStarted;

    private void OnGameStarted()
    {
        GetPlayers();

        StartCoroutine(InstantiateCoroutine());
    }

    private void GetPlayers()
    {
        _player1 = GlobalFunctions.ObjectsOfType<PlayerTurn>.Find(turn => turn.MyTurn == TurnState.Player1).transform;
        _player2 = GlobalFunctions.ObjectsOfType<PlayerTurn>.Find(turn => turn.MyTurn == TurnState.Player2).transform;
    }

    private IEnumerator InstantiateCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0, 5));

            if (FindObjectOfType<ParachuteWithWoodBoxController>() == null && _player1 != null && _player2 != null)
                InstantiateWoodBox(WoodBoxSpawnPosition());
        }
    }

    private void InstantiateWoodBox(Vector3 position)
    {
        float randomTime = Random.Range(30, 90);

        if (MyPhotonNetwork.IsOfflineMode)
            InstantiateWoodBox(position, randomTime);

        if (MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer))
            photonView.RPC("InstantiateWoodBoxRPC", RpcTarget.AllViaServer, position, randomTime);
    }

    private void InstantiateWoodBox(Vector3 position, float randomTime)
    {
        _parachuteWithWoodBoxController = Instantiate(_woodBoxControllerPrefab, position, Quaternion.identity);

        _parachuteWithWoodBoxController.Id = position;

        _parachuteWithWoodBoxController.RandomDestroyTime = randomTime;
    }

    [PunRPC]
    private void InstantiateWoodBoxRPC(Vector3 position, float randomTime)
    {
        InstantiateWoodBox(position, randomTime);
    }

    private Vector3 WoodBoxSpawnPosition()
    {
        Vector3 tempPosition = new Vector3(Random.Range(_player1.position.x, _player2.position.x), 5, 0);

        Vector3 tilePosition = tempPosition;

        foreach (var tileDict in GameSceneObjectsReferences.TilesData.TilesDict)
        {
            if (tileDict.Key.x >= tempPosition.x - 1 && tileDict.Key.x <= tempPosition.x + 1)
            {
                tilePosition.x = tileDict.Key.x;

                break;
            }
        }

        return tilePosition;
    }
}
