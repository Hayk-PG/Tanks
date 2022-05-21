using UnityEngine;

public class MessageTypes : MonoBehaviour
{
    private Tab_Message _tabMessage;
    private Tab_InRoom _tabInRoom;


    private void Awake()
    {
        _tabMessage = Get<Tab_Message>.From(gameObject);
        _tabInRoom = FindObjectOfType<Tab_InRoom>();
    }

    public void LeaveRoomMessage()
    {
        _tabMessage.OnMessage(MyPhoton.LeaveRoom, _tabInRoom.OpenTab, MessageType.Error, Messages.LeaveRoomMessages);
    }
}
