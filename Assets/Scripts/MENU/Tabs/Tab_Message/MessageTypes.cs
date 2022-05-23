using UnityEngine;

public class MessageTypes : MonoBehaviour
{
    private Tab_Message _tabMessage;
    private Tab_StartGame _tabStartGame;
    private Tab_InRoom _tabInRoom;
    private Tab_SelectedTanks _tabSelectedTanks;


    private void Awake()
    {
        _tabMessage = Get<Tab_Message>.From(gameObject);
        _tabStartGame = FindObjectOfType<Tab_StartGame>();
        _tabInRoom = FindObjectOfType<Tab_InRoom>();
        _tabSelectedTanks = FindObjectOfType<Tab_SelectedTanks>();
    }

    public void LeaveRoomMessage()
    {
        _tabMessage.OnMessage(MyPhoton.LeaveRoom, _tabInRoom.OpenTab, MessageType.Error, Messages.LeaveRoomMessages);
    }

    public void CouldntOpenProfileTabMessage()
    {
        _tabMessage.OnMessage(_tabStartGame.OpenTab, _tabSelectedTanks.OpenTab, MessageType.Error, Messages.ActionInOfflineModeErrorMessage);
    }
}
