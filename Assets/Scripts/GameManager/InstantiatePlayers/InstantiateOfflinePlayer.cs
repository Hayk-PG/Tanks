using UnityEngine;

public class InstantiateOfflinePlayer : MonoBehaviour
{
    private GameManager _gameManager;
    private OfflinePlayer _offlinePlayer;


    private void Awake()
    {
        _gameManager = Get<GameManager>.From(gameObject);
        _offlinePlayer = Resources.Load<OfflinePlayer>("OfflinePlayer");
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
        OfflinePlayer offlinePlayer = Instantiate(_offlinePlayer);
    }
}
