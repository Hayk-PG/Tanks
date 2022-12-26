using UnityEngine;

public class Tab_Endgame3 : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private GameResultProcessor _gameResultProcessor;


    private void Awake()
    {
        _canvasGroup = Get<CanvasGroup>.From(gameObject);
        _gameResultProcessor = Get<GameResultProcessor>.From(gameObject);
    }

    private void OnEnable() => _gameResultProcessor.onBeginResultProcess += OnBeginResultProcess;

    private void OnDisable() => _gameResultProcessor.onBeginResultProcess -= OnBeginResultProcess;

    private void OnBeginResultProcess() => GlobalFunctions.CanvasGroupActivity(_canvasGroup, true);
}
