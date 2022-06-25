using System;
using UnityEngine;
using UnityEngine.UI;

public class Tab_EndgameChat : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroupChat;
    [SerializeField] private CanvasGroup _canvasGroupTabResult;
    [SerializeField] private Button _buttonChat;
    [SerializeField] private Button buttonDimed;

    public bool IsChatOpened
    {
        get => _canvasGroupChat.interactable;
    }
    public Action OnChatOpen { get; set; }



    private void Update()
    {
        _buttonChat.onClick.RemoveAllListeners();
        _buttonChat.onClick.AddListener(delegate
        {
            GlobalFunctions.CanvasGroupActivity(_canvasGroupChat, true);
            GlobalFunctions.CanvasGroupActivity(_canvasGroupTabResult, false);
            OnChatOpen?.Invoke();
        });

        buttonDimed.onClick.RemoveAllListeners();
        buttonDimed.onClick.AddListener(delegate 
        {
            GlobalFunctions.CanvasGroupActivity(_canvasGroupChat, false);
            GlobalFunctions.CanvasGroupActivity(_canvasGroupTabResult, true);
        });
    }
}
