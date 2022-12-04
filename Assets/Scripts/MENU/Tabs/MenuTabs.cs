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
    }

    public static void Activity(CanvasGroup activeCanvasGroup)
    {
        Tab_StartGame.CloseTab();
        Tab_SignUp.CloseTab();
        Tab_SignIn.CloseTab();
        Tab_HomeOnline.CloseTab();
        Tab_HomeOffline.CloseTab();
        Tab_SelectLobby.CloseTab();
        Tab_Room.CloseTab();
        Tab_Tanks.CloseTab();
        Tab_AITanks.CloseTab();

        if (activeCanvasGroup != null) GlobalFunctions.CanvasGroupActivity(activeCanvasGroup, true);
    }
}
