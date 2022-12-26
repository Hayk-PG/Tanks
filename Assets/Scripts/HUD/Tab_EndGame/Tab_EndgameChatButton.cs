using UnityEngine;

public class Tab_EndgameChatButton : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private Chat _chat;
    private GameResultProcessor _gameResultProcessor;
    private Tab_EndgameChat _tabEndgameChat;   
    [SerializeField] private GameObject _notification;


    private void Awake()
    {
        _canvasGroup = Get<CanvasGroup>.From(gameObject);
        _chat = FindObjectOfType<Chat>();
        _gameResultProcessor = FindObjectOfType<GameResultProcessor>();
        _tabEndgameChat = FindObjectOfType<Tab_EndgameChat>();
    }

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
