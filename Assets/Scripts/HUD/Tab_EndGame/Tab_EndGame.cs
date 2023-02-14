using UnityEngine;

public class Tab_EndGame : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup _canvasGroup;

    [SerializeField] [Space]
    private GameResultProcessor _gameResultProcessor;


 
    private void OnEnable() => _gameResultProcessor.onBeginResultProcess += OnBeginResultProcess;

    private void OnDisable() => _gameResultProcessor.onBeginResultProcess -= OnBeginResultProcess;

    private void OnBeginResultProcess()
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, true);
    }
}
