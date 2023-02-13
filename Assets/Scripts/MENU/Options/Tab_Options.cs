using UnityEngine;
using TMPro;


public class Tab_Options : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private Options _options;

    [SerializeField]
    private TMP_Text _txtTitle;


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
        SetTitle(isActive);
    }

    private void SetTitle(bool isActive)
    {
        if (isActive)
            _txtTitle.text = MyPhotonNetwork.IsOfflineMode ? "offline" : "online";
    }
}
