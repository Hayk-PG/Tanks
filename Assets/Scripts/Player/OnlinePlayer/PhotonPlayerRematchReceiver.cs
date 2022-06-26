using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PhotonPlayerRematchReceiver : PhotonPlayerBaseRPC
{
    [SerializeField] private bool _isReadyToRematch;
    public bool IsReadyToRematch
    {
        get => _isReadyToRematch;
        set => _isReadyToRematch = value;
    }
    private bool _isSubscribesToPluginService;
    private Tab_EndgameRematchButton _rematchButton;
    private Chat _chat;
    private MyPlugins _myPlugins;
    private PhotonPlayerRematchReceiver _otherRematchReceiver;
    private Network _network;
    private ChatRematchMessage _rematchMessage;


    protected override void Awake()
    {
        base.Awake();
        _rematchButton = FindObjectOfType<Tab_EndgameRematchButton>();
        _chat = FindObjectOfType<Chat>();
        _myPlugins = FindObjectOfType<MyPlugins>();
        _network = FindObjectOfType<Network>();
    }

    private void OnEnable()
    {
        if (_photonPlayerController.PhotonView.IsMine)
        {
            _rematchButton.OnRematch += OnRematch;
            _network.OnLoadLevelRPC += OnLoadLevelRPC;
        }           
    }

    private void OnDisable()
    {
        _rematchButton.OnRematch -= OnRematch;
        _network.OnLoadLevelRPC -= OnLoadLevelRPC;        
        UnsubscribeFromPluginService();
        if (_rematchMessage != null)
            _rematchMessage.OnRespond -= OnRematchMessageRespond;
    }

    private void UnsubscribeFromPluginService()
    {
        _myPlugins.OnPluginService -= OnPluginService;
    }

    private void SubscribeToPluginService()
    {
        if (!_isSubscribesToPluginService)
        {
            _myPlugins.OnPluginService += OnPluginService;
            _isSubscribesToPluginService = false;
        }
    }

    private void InitializeOtherRematchReceiver()
    {
        if (_otherRematchReceiver == null) _otherRematchReceiver = GlobalFunctions.ObjectsOfType<PhotonPlayerRematchReceiver>.Find(r => r != this);
    }

    private void OnRematch()
    {
        _photonPlayerController.PhotonView.RPC("SendNotificationRPC", RpcTarget.AllViaServer, MyPhotonNetwork.LocalPlayer);
        InitializeOtherRematchReceiver();
        SubscribeToPluginService();
    }

    [PunRPC]
    public void SendNotificationRPC(Player player)
    {
        PhotonPlayerRematchReceiver senderRematchReceiver = GlobalFunctions.ObjectsOfType<PhotonPlayerRematchReceiver>.Find(rematchReceiver => Get<PhotonPlayerController>.From(rematchReceiver.gameObject).ActorNumber == player.ActorNumber);
        PhotonPlayerRematchReceiver receiverRematchReceiver = GlobalFunctions.ObjectsOfType<PhotonPlayerRematchReceiver>.Find(rematchReceiver => Get<PhotonPlayerController>.From(rematchReceiver.gameObject).ActorNumber != player.ActorNumber);

        if (senderRematchReceiver != null) senderRematchReceiver.IsReadyToRematch = true;
        if (receiverRematchReceiver != null)
        {
            if (!receiverRematchReceiver.IsReadyToRematch)
                receiverRematchReceiver.CreateRematchMessageAndSubscribeToIt(player);
        }        
    }

    public void CreateRematchMessageAndSubscribeToIt(Player player)
    {
        if (_photonPlayerController.PhotonView.IsMine)
        {
            _chat?.InstantiateRematchMessage(player.NickName);
            _rematchMessage = FindObjectOfType<ChatRematchMessage>();
            if (_rematchMessage != null)
                _rematchMessage.OnRespond += OnRematchMessageRespond;
        }
    }

    private void OnRematchMessageRespond(bool isAccepted)
    {
        if (isAccepted)
        {
            SubscribeToPluginService();
            _photonPlayerController.PhotonView.RPC("SendNotificationRPC", RpcTarget.AllViaServer, MyPhotonNetwork.LocalPlayer);
        }
        else
        {
            string system = "System| ";
            string chatText = "The opponent has declined a rematch!";
            _photonPlayerController.PhotonView.RPC("InstantiateTextRPC", RpcTarget.AllViaServer, system, chatText, 2);
        }
    }

    private void OnPluginService()
    {
        if (_otherRematchReceiver != null)
        {
            if (_otherRematchReceiver.IsReadyToRematch)
            {
                _network.LoadLevelRPC(MyPhotonNetwork.LocalPlayer);               
                UnsubscribeFromPluginService();
            }
        }
        else
        {
            InitializeOtherRematchReceiver();
        }
    }

    private void OnLoadLevelRPC(Player player)
    {
        if (player.IsMasterClient)
            MyPhotonNetwork.LoadLevel();
    }
}
