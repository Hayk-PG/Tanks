using UnityEngine;

public class InstantiateOfflinePlayer : MonoBehaviour
{
    [SerializeField]
    private GameManager _gameManager;

    [SerializeField]
    private OfflinePlayerController _offlinePlayer;


    private void OnEnable() => _gameManager.OnInstantiateOfflinePlayers += Instantiate;

    private void OnDisable() => _gameManager.OnInstantiateOfflinePlayers -= Instantiate;

    private void Instantiate()
    {
        OfflinePlayerController offlinePlayer = Instantiate(_offlinePlayer);
        offlinePlayer.Initialize();
    }
}
