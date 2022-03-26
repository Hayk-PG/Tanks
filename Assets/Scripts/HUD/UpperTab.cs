using UnityEngine;

public class UpperTab : MonoBehaviour
{
    private GameManager _gameManager;
    private CanvasGroup _canvasGroup;



    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _canvasGroup = GetComponent<CanvasGroup>();
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
        if(_canvasGroup != null) GlobalFunctions.CanvasGroupActivity(_canvasGroup, true);
    }
}
