using UnityEngine;

public class Tab_GameStart : MonoBehaviour
{
    [SerializeField] [Space]
    private GameManager _gameManager;



    private void OnEnable() => _gameManager.OnGameStarted += OnGameStarted;

    private void OnDisable() => _gameManager.OnGameStarted -= OnGameStarted;

    private void OnGameStarted() => gameObject.SetActive(false);
}
