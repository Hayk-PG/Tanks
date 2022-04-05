using System;
using UnityEngine;

public class Tab_StartGame : MonoBehaviour
{
    public CanvasGroup CanvasGroup { get; set; }
    public Action OnStartPlayVsAi { get; set; }
    public Action OnStartPlayVsOtherPlayer { get; set; }


    private void Awake()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnClickVsAiButton()
    {
        OnStartPlayVsAi?.Invoke();
    }

    public void OnClickVsPlayerButton()
    {
        OnStartPlayVsOtherPlayer?.Invoke();
    }
}
