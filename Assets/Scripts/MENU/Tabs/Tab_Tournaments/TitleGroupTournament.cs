using System;
using UnityEngine;

public class TitleGroupTournament : MonoBehaviour
{
    [SerializeField]
    private TournamentButton[] tournamentButton;

    private MyPlayfabTitleGroupsEntityKeys _myPlayfabTitleGroupsEntityKeys;

    public event Action<TitleProperties> onAttemptToJoinTournamentLobby;


    private void Awake()
    {
        _myPlayfabTitleGroupsEntityKeys = FindObjectOfType<MyPlayfabTitleGroupsEntityKeys>();
    }

    private void OnEnable()
    {
        _myPlayfabTitleGroupsEntityKeys.onShareEntities += OnTitleGroupEntitiesReceived;

        GlobalFunctions.Loop<TournamentButton>.Foreach(tournamentButton, button =>
        {
            button.onPressTurnamentButton += OnTurnamentButtonPressed;
        });
    }

    private void OnDisable()
    {
        _myPlayfabTitleGroupsEntityKeys.onShareEntities -= OnTitleGroupEntitiesReceived;

        GlobalFunctions.Loop<TournamentButton>.Foreach(tournamentButton, button =>
        {
            button.onPressTurnamentButton -= OnTurnamentButtonPressed;
        });
    }

    private void OnTitleGroupEntitiesReceived(MyPlayfabTitleGroupsEntityKeys myPlayfabTitleGroupsEntityKeys)
    {
        for (int i = 0; i < myPlayfabTitleGroupsEntityKeys.GroupsID.Length; i++)
        {
            tournamentButton[i].Initialize(new TitleProperties(myPlayfabTitleGroupsEntityKeys.GroupsID[i], myPlayfabTitleGroupsEntityKeys.GroupsType, null, null));
        }
    }

    private void OnTurnamentButtonPressed(TitleProperties titleGroupProperties)
    {
        onAttemptToJoinTournamentLobby?.Invoke(titleGroupProperties);
    }
}
