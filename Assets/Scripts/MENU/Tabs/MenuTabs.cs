using UnityEngine;

public class MenuTabs : MonoBehaviour
{
    public static Tab_StartGame Tab_StartGame { get; private set; }
    public static Tab_SignUp Tab_SignUp { get; private set; }
    public static Tab_SignIn Tab_SignIn { get; private set; }
    public static Tab_SelectedTanks Tab_SelectedTanks { get; private set; }
    public static Tab_SelectedAITanks Tab_SelectedAITanks { get; private set; }
    public static Tab_SelectMaps Tab_SelectMap { get; private set; }
    public static Tab_Lobby Tab_Lobby { get; private set; }
    public static Tab_InRoom Tab_InRoom { get; private set; }
    public static Tab_Message Tab_Message { get; private set; }
    public static Tab_Profile Tab_Profile { get; private set; }
    public static Tab_Tournaments Tab_Tournaments { get; private set; }


    private void Awake()
    {
        Tab_StartGame = Get<Tab_StartGame>.FromChild(gameObject);
        Tab_SignUp = Get<Tab_SignUp>.FromChild(gameObject);
        Tab_SignIn = Get<Tab_SignIn>.FromChild(gameObject);
        Tab_SelectedTanks = Get<Tab_SelectedTanks>.FromChild(gameObject);
        Tab_SelectedAITanks = Get<Tab_SelectedAITanks>.FromChild(gameObject);
        Tab_SelectMap = Get<Tab_SelectMaps>.FromChild(gameObject);
        Tab_Lobby = Get<Tab_Lobby>.FromChild(gameObject);
        Tab_InRoom = Get<Tab_InRoom>.FromChild(gameObject);
        Tab_Message = Get<Tab_Message>.FromChild(gameObject);
        Tab_Profile = Get<Tab_Profile>.FromChild(gameObject);
        Tab_Tournaments = Get<Tab_Tournaments>.FromChild(gameObject);
    }

    public static void Activity(CanvasGroup activeCanvasGroup)
    {
        GlobalFunctions.CanvasGroupActivity(Tab_StartGame.CanvasGroup, false);
        GlobalFunctions.CanvasGroupActivity(Tab_SignUp.CanvasGroup, false);
        GlobalFunctions.CanvasGroupActivity(Tab_SignIn.CanvasGroup, false);
        GlobalFunctions.CanvasGroupActivity(Tab_SelectedTanks.CanvasGroup, false);
        GlobalFunctions.CanvasGroupActivity(Tab_SelectedAITanks.CanvasGroup, false);
        GlobalFunctions.CanvasGroupActivity(Tab_SelectMap.CanvasGroup, false);
        GlobalFunctions.CanvasGroupActivity(Tab_Lobby.CanvasGroup, false);
        GlobalFunctions.CanvasGroupActivity(Tab_InRoom.CanvasGroup, false);
        GlobalFunctions.CanvasGroupActivity(Tab_Message.CanvasGroup, false);
        GlobalFunctions.CanvasGroupActivity(Tab_Profile.CanvasGroup, false);
        GlobalFunctions.CanvasGroupActivity(Tab_Tournaments.CanvasGroup, false);

        if (activeCanvasGroup != null) GlobalFunctions.CanvasGroupActivity(activeCanvasGroup, true);
    }
}
