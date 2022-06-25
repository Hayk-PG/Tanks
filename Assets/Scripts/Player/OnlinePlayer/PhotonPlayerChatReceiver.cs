using Photon.Pun;

public class PhotonPlayerChatReceiver : PhotonPlayerBaseRPC
{
    private Chat _chat;
    private int _chatTextColorIndex;


    protected override void Awake()
    {
        base.Awake();
        _chat = FindObjectOfType<Chat>();        
    }

    private void Start()
    {
        _chatTextColorIndex = MyPhotonNetwork.LocalPlayer.IsMasterClient ? 0 : 1;
    }

    private void OnEnable()
    {
        if (_photonPlayerController.PhotonView.IsMine)
            _chat.OnSend += OnSend;
    }

    private void OnDisable()
    { 
        _chat.OnSend -= OnSend;
    }

    private void OnSend(string chatText)
    {
        _photonPlayerController.PhotonView.RPC("InstantiateTextRPC", RpcTarget.AllViaServer, MyPhotonNetwork.LocalPlayer.NickName, chatText, _chatTextColorIndex);
    }

    [PunRPC]
    public void InstantiateTextRPC(string senderName, string chatText, int playerIndex)
    {
        _chat?.InstantiateText(senderName, chatText, playerIndex);
    }
}
