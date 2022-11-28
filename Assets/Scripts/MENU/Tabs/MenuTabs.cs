using UnityEngine;

public class MenuTabs : MonoBehaviour
{
    public static Tab_StartGame Tab_StartGame { get; private set; }
    public static Tab_SignUp Tab_SignUp { get; private set; }
    public static Tab_SignIn Tab_SignIn { get; private set; }
    public static Tab_HomeOnline Tab_HomeOnline { get; private set; }
    public static Tab_HomeOffline Tab_HomeOffline { get; private set; }
    public static Tab_SelectLobby Tab_SelectLobby { get; private set; }
    public static Tab_Room Tab_Room { get; private set; }
    public static Tab_Tanks Tab_Tanks { get; private set; }
    public static Tab_AITanks Tab_AITanks { get; private set; }
    //public static Tab_Tanks Tab_Tanks { get; private set; }
    //public static Tab_AITanks Tab_AITanks { get; private set; }
    //public static Tab_Lobby Tab_Lobby { get; private set; }
    //public static Tab_InRoom Tab_InRoom { get; private set; }
    //public static Tab_Message Tab_Message { get; private set; }
    //public static Tab_Matchmake Tab_Matchmake { get; private set; }


    private void Awake()
    {
        Tab_StartGame = Get<Tab_StartGame>.FromChild(gameObject);
        Tab_SignUp = Get<Tab_SignUp>.FromChild(gameObject);
        Tab_SignIn = Get<Tab_SignIn>.FromChild(gameObject);
        Tab_HomeOnline = Get<Tab_HomeOnline>.FromChild(gameObject);
        Tab_HomeOffline = Get<Tab_HomeOffline>.FromChild(gameObject);
        Tab_SelectLobby = Get<Tab_SelectLobby>.FromChild(gameObject);
        Tab_Room = Get<Tab_Room>.FromChild(gameObject);
        Tab_Tanks = Get<Tab_Tanks>.FromChild(gameObject);
        Tab_AITanks = Get<Tab_AITanks>.FromChild(gameObject);
        //Tab_Tanks = Get<Tab_Tanks>.FromChild(gameObject);
        //Tab_AITanks = Get<Tab_AITanks>.FromChild(gameObject);
        //Tab_Lobby = Get<Tab_Lobby>.FromChild(gameObject);
        //Tab_InRoom = Get<Tab_InRoom>.FromChild(gameObject);
        //Tab_Message = Get<Tab_Message>.FromChild(gameObject);
        //Tab_Matchmake = Get<Tab_Matchmake>.FromChild(gameObject);
    }

    public static void Activity(CanvasGroup activeCanvasGroup)
    {
        GlobalFunctions.CanvasGroupActivity(Tab_StartGame.CanvasGroup, false);
        GlobalFunctions.CanvasGroupActivity(Tab_SignUp.CanvasGroup, false);
        GlobalFunctions.CanvasGroupActivity(Tab_SignIn.CanvasGroup, false);
        GlobalFunctions.CanvasGroupActivity(Tab_HomeOnline.CanvasGroup, false);
        GlobalFunctions.CanvasGroupActivity(Tab_HomeOffline.CanvasGroup, false);
        GlobalFunctions.CanvasGroupActivity(Tab_SelectLobby.CanvasGroup, false);
        GlobalFunctions.CanvasGroupActivity(Tab_Room.CanvasGroup, false);
        GlobalFunctions.CanvasGroupActivity(Tab_Tanks.CanvasGroup, false);
        GlobalFunctions.CanvasGroupActivity(Tab_AITanks.CanvasGroup, false);
        //GlobalFunctions.CanvasGroupActivity(Tab_Tanks.CanvasGroup, false);
        //GlobalFunctions.CanvasGroupActivity(Tab_AITanks.CanvasGroup, false);
        //GlobalFunctions.CanvasGroupActivity(Tab_Lobby.CanvasGroup, false);
        //GlobalFunctions.CanvasGroupActivity(Tab_InRoom.CanvasGroup, false);
        //GlobalFunctions.CanvasGroupActivity(Tab_Message.CanvasGroup, false);
        //GlobalFunctions.CanvasGroupActivity(Tab_Matchmake.CanvasGroup, false);

        if (activeCanvasGroup != null) GlobalFunctions.CanvasGroupActivity(activeCanvasGroup, true);
    }
}
