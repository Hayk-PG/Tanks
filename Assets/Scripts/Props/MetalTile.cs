using UnityEngine;

public class MetalTile : MonoBehaviour
{
    private GameManager _gameManager;


    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();

        if (_gameManager.IsGameStarted)
            SecondarySoundController.PlaySound(1, 2);
    }
}
