using UnityEngine;

public class InstantiateOfflinePlayer : MonoBehaviour
{
    private GameManager _gameManager;
    private OfflinePlayerController _offlinePlayer;


    private void Awake()
    {
        _gameManager = Get<GameManager>.From(gameObject);
        _offlinePlayer = Resources.Load<OfflinePlayerController>("OfflinePlayer");
    }

    private void OnEnable()
    {
        _gameManager.OnInstantiateOfflinePlayers += Instantiate;
    }

    private void OnDisable()
    {
        _gameManager.OnInstantiateOfflinePlayers -= Instantiate;
    }

    private void Instantiate()
    {
        OfflinePlayerController offlinePlayer = Instantiate(_offlinePlayer);
        offlinePlayer.Initialize();
    }
}
