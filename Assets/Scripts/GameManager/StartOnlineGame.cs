using Photon.Pun;

public class StartOnlineGame : MonoBehaviourPun
{
    private GameManager _gameManager;


    private void Awake()
    {
        _gameManager = Get<GameManager>.From(gameObject);
    }

    private void Update()
    {       
        if (photonView.IsMine && !MyPhotonNetwork.IsOfflineMode && !GameManager.IsGameStarted)
        {
            photonView.RPC("OnPlayersReadyStartTheGame", RpcTarget.AllViaServer);
        }
    }

    [PunRPC]
    private void OnPlayersReadyStartTheGame()
    {
        if(GameManager.MasterPlayerReady && GameManager.SecondPlayerReady)
        {
            _gameManager.OnGameStarted?.Invoke();
            GameManager.IsGameStarted = true;
        }
    }
}
