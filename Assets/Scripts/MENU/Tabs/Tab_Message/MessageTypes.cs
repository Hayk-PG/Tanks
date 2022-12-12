using UnityEngine;
using UnityEngine.Events;

public class MessageTypes : MonoBehaviour
{
    private Tab_Message _tabMessage;
    private Tab_StartGame _tabStartGame;
    private Tab_Room _tabInRoom;



    private void Awake()
    {
        _tabMessage = Get<Tab_Message>.From(gameObject);
        _tabStartGame = FindObjectOfType<Tab_StartGame>();
        _tabInRoom = FindObjectOfType<Tab_Room>();
    }

    public void LeaveRoomMessage()
    {
        _tabMessage.OnMessage(MyPhoton.LeaveRoom, _tabInRoom.OpenTab, MessageType.Error, Messages.LeaveRoomMessages);
    }

    public void CouldntOpenProfileTabMessage(UnityEvent OnClickNo)
    {
        _tabMessage.OnMessage(_tabStartGame.OpenTab, delegate { OnClickNo?.Invoke(); }, MessageType.Error, Messages.ActionInOfflineModeErrorMessage);
    }
}
