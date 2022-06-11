using UnityEngine;

public class TerOccIndButton : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private CanvasGroup _upperTabCanvasGroup;
    private GameManager _gameManager;

    private bool _isOpened;

    private void Awake()
    {
        _canvasGroup = Get<CanvasGroup>.FromChild(gameObject);
        _upperTabCanvasGroup = Get<CanvasGroup>.From(FindObjectOfType<UpperTab>().gameObject);
        _gameManager = FindObjectOfType<GameManager>();
    }

    public void OnButtonClick()
    {
        if (_gameManager.IsGameStarted)
        {
            _isOpened = !_isOpened;
            GlobalFunctions.CanvasGroupActivity(_upperTabCanvasGroup, !_isOpened);
            GlobalFunctions.CanvasGroupActivity(_canvasGroup, _isOpened);
        }
    }
}
