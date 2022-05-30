using UnityEngine;

public class TopBarBackButtonFromTab_SelectedTanks : MonoBehaviour
{
    private Tab_StartGame _tabStartGame;
    private Tab_InRoom _tabInRoom;


    private void Awake()
    {
        _tabStartGame = FindObjectOfType<Tab_StartGame>();
        _tabInRoom = FindObjectOfType<Tab_InRoom>();
    }

    public void OnTopBarBackButton()
    {
        if (MyPhotonNetwork.IsInRoom)
            _tabInRoom.OpenTab();
        else
            _tabStartGame.OpenTab();
    }
}
