using UnityEngine;

public class Tab_Options : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private Options _options;


    private void Awake()
    {
        _canvasGroup = Get<CanvasGroup>.From(gameObject);
        _options = Get<Options>.FromChild(gameObject);
    }

    private void OnEnable()
    {
        _options.onOptionsActivity += GetOptionsActivity;
    }

    private void OnDisable()
    {
        _options.onOptionsActivity += GetOptionsActivity;
    }

    private void GetOptionsActivity(bool isActive)
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, isActive);
    }
}
