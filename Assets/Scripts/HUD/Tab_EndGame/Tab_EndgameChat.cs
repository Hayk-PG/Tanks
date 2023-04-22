using System;
using UnityEngine;

public class Tab_EndgameChat : MonoBehaviour
{
    [SerializeField] 
    private CanvasGroup _canvasGroupChat, _canvasGroupTabResult;

    public bool IsChatOpened
    {
        get => _canvasGroupChat.interactable;
    }
    public Action OnChatOpen { get; set; }


    public void OpenChatTab()
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroupChat, true);
        GlobalFunctions.CanvasGroupActivity(_canvasGroupTabResult, false);

        OnChatOpen?.Invoke();
    }

    public void CloseChatTab()
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroupChat, false);
        GlobalFunctions.CanvasGroupActivity(_canvasGroupTabResult, true);
    }
}
