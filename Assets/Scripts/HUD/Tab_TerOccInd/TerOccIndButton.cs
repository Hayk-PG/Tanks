using UnityEngine;

public class TerOccIndButton : MonoBehaviour
{
    [SerializeField] [Space]
    private CanvasGroup _canvasGroup, _upperTabCanvasGroup;

    [SerializeField] [Space]
    private GameManager _gameManager;

    private bool _isOpened;


    

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
