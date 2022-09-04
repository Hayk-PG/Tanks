using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;

public class InstantiatePickables : MonoBehaviourPun
{
    private GameManager _gameManager;
    private Transform _player1, _player2;
   
    private bool IsThereOtherActiveParachute
    {
        get => FindObjectOfType<ParachuteWithWoodBoxController>();
    }
    public Vector3 RandomSpawnPosition { get; set; }    
    public float RandomTime { get; set; }
    public int RandomContent { get; set; }

    public System.Action<object[]> OnInstantiateWoodenBox { get; set; }



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

    private IEnumerator InstantiateCoroutine()
    {
        while (true)
        {
            RandomTime = Random.Range(30, 120);
            yield return new WaitForSeconds(RandomTime);

            RandomSpawnPosition = new Vector3(Random.Range(_player1.position.x, _player2.position.x), 5, 0);           
            RandomContent = Random.Range(0, 4);

            if (MyPhotonNetwork.IsOfflineMode || !MyPhotonNetwork.IsOfflineMode && MyPhotonNetwork.AmPhotonViewOwner(photonView))
            {
                if (_player1 != null && _player2 != null && !IsThereOtherActiveParachute)
                {
                    EventInfo.Content_InstantiateWoodenBox = new object[]
                    {
                    RandomSpawnPosition,
                    RandomContent
                    };

                    //ONLINE
                    PhotonNetwork.RaiseEvent(EventInfo.Code_InstantiateWoodenBox, EventInfo.Content_InstantiateWoodenBox, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
                    //OFFLINE
                    OnInstantiateWoodenBox?.Invoke(EventInfo.Content_InstantiateWoodenBox);
                }
            }
        }
    }
}
