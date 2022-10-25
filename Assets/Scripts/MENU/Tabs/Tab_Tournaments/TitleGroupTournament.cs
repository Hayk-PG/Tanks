using System;
using UnityEngine;

public class TitleGroupTournament : MonoBehaviour
{
    [SerializeField]
    private TournamentButton[] tournamentButton;
    private Tab_Tournaments _tabTournaments;

    public event Action<TitleProperties> onAttemptToJoinTournamentLobby;


    private void Awake()
    {
        _tabTournaments = Get<Tab_Tournaments>.From(gameObject);
    }

    private void OnEnable()
    {
        GlobalFunctions.Loop<TournamentButton>.Foreach(tournamentButton, button =>
        {
            button.onPressTurnamentButton += OnTurnamentButtonPressed;
        });

        _tabTournaments.onShareTournamentsGroupsProperties += ReceiveSharedTournamentsGroupsProperties; 
    }

    private void OnDisable()
    {
        GlobalFunctions.Loop<TournamentButton>.Foreach(tournamentButton, button =>
        {
            button.onPressTurnamentButton -= OnTurnamentButtonPressed;
        });

        _tabTournaments.onShareTournamentsGroupsProperties -= ReceiveSharedTournamentsGroupsProperties;
    }

    private void OnTurnamentButtonPressed(TitleProperties titleGroupProperties)
    {
        onAttemptToJoinTournamentLobby?.Invoke(titleGroupProperties);
    }

    private void ReceiveSharedTournamentsGroupsProperties(string[] groupIds, string groupType)
    {
        for (int i = 0; i < groupIds.Length; i++)
        {
            tournamentButton[i].Initialize(new TitleProperties(groupIds[i], groupType, null, null));
        }
    }
}
