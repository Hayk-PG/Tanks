using UnityEngine;
using TMPro;
using System;

//USED COMMENTS
public class Tab_Options : MonoBehaviour
{
    private CanvasGroup _canvasGroup;

    private Options _options;

    [SerializeField]
    private TMP_Text _txtTitle;

    // GetOptionsActivityHolder is used for public access to GetOptionsActivity
    public Action<bool> GetOptionsActivityHolder => GetOptionsActivity;


    private void Awake()
    {
        _canvasGroup = Get<CanvasGroup>.From(gameObject);

        _options = Get<Options>.FromChild(gameObject);
    }

    private void OnEnable() => _options.onOptionsActivity += GetOptionsActivity;

    private void OnDisable() => _options.onOptionsActivity += GetOptionsActivity;

    // This method is held by GetOptionsActivityHolder delegate for public use only
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
