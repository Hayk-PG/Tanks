using UnityEngine;

public class Tab_EndgameChatButton : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup _canvasGroup;

    [SerializeField] [Space]
    private Chat _chat;

    [SerializeField] [Space]
    private GameResultProcessor _gameResultProcessor;

    [SerializeField] [Space]
    private Tab_EndgameChat _tabEndgameChat;  
    
    [SerializeField] [Space]
    private GameObject _notification;




    private void OnEnable()
    {
        _chat.OnTextInstantiated += OnTextInstantiated;
        _gameResultProcessor.onFinishResultProcess += ShowChatButton;
        _tabEndgameChat.OnChatOpen += OnChatOpen;
    }

    private void OnDisable()
    {
        _chat.OnTextInstantiated -= OnTextInstantiated;
        _gameResultProcessor.onFinishResultProcess -= ShowChatButton;
        _tabEndgameChat.OnChatOpen -= OnChatOpen;
    }

    private void ShowChatButton()
    {
        if (MyPhotonNetwork.IsOfflineMode)
            return;
        else
            GlobalFunctions.CanvasGroupActivity(_canvasGroup, true);
    }

    private void OnTextInstantiated(bool isChatActive)
    {
        _notification.SetActive(!isChatActive);
    }

    private void OnChatOpen()
    {
        _notification.SetActive(false);
    }
}
