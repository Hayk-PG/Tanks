using System;
using UnityEngine;

public class TitleGroupTournament : MonoBehaviour
{
    [SerializeField]
    private TournamentButton[] tournamentButton;
    private Tab_Tournaments _tabTournaments;

    public event Action<TitleProperties> onClickTournamentLobbyButton;


    private void Awake()
    {
        _tabTournaments = Get<Tab_Tournaments>.From(gameObject);
    }

    private void OnEnable()
    {
        _tabTournaments.onShareTournamentsGroupsProperties += ReceiveSharedTournamentsGroupsProperties;

        GlobalFunctions.Loop<TournamentButton>.Foreach(tournamentButton, button => { button.onPressTurnamentButton += OnTurnamentButtonPressed; });
    }

    private void OnDisable()
    {
        _tabTournaments.onShareTournamentsGroupsProperties -= ReceiveSharedTournamentsGroupsProperties;

        GlobalFunctions.Loop<TournamentButton>.Foreach(tournamentButton, button => { button.onPressTurnamentButton -= OnTurnamentButtonPressed; });
    }

    private void ReceiveSharedTournamentsGroupsProperties(string[] groupIds, string groupType)
    {
        for (int i = 0; i < groupIds.Length; i++)
        {
            tournamentButton[i].Initialize(new TitleProperties(groupIds[i], groupType, null, null));
        }
    }

    private void OnTurnamentButtonPressed(TitleProperties titleGroupProperties)
    {
        onClickTournamentLobbyButton?.Invoke(titleGroupProperties);
    }
}
