using System;

public class Tab_Tournaments : Tab_Base<OnlineGameModeController>
{
    public string[] GroupsID { get; private set; }
    public string GroupsType { get; private set; }


    public event Action<string[], string> onShareTournamentsGroupsProperties;


    protected override void Awake()
    {
        base.Awake();
        SetGroupsIDs();
    }

    private void OnEnable()
    {
        _object.onSelectTournametMode += OnTournamentModeSelected;
    }

    private void OnDisable()
    {
        _object.onSelectTournametMode -= OnTournamentModeSelected;
    }

    private void SetGroupsIDs()
    {
        GroupsID = new string[] { "CBE02DC5CDA093FF" };
        GroupsType = "title_player_account";
    }

    private void OnTournamentModeSelected()
    {
        base.OpenTab();
        onShareTournamentsGroupsProperties?.Invoke(GroupsID, GroupsType);
    }
}
