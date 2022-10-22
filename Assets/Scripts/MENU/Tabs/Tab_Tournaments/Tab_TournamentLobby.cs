using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;

public class Tab_TournamentLobby : MonoBehaviourPun
{
    private CanvasGroup _canvasGroup;
    private TitleGroupTournament _titleGroupTournament;
    private MyPhotonCallbacks _myPhotonCallbacks;

    private string _groupId = "empty"; // NAME OF THE CURRENT LOBBY
    private string _groupType = "empty";

    private bool IsCorrectLobby
    {
        get => MyPhotonNetwork.CurrentLobby.Name == _groupId;
    }

    public event Action<TitleGroupProperties> onTournamentLobbyJoined;


    private void Awake()
    {
        _canvasGroup = Get<CanvasGroup>.From(gameObject);
        _titleGroupTournament = FindObjectOfType<TitleGroupTournament>();
        _myPhotonCallbacks = FindObjectOfType<MyPhotonCallbacks>();
    }

    private void OnEnable()
    {
        _titleGroupTournament.onAttemptToJoinTournamentLobby += OnJoinTournamentLobbyAttempted;
        _myPhotonCallbacks._OnJoinedLobby += OnJoinedLobby;
        _myPhotonCallbacks._OnLeftLobby += OnLeftLobby;
    }

    private void OnDisable()
    {
        _titleGroupTournament.onAttemptToJoinTournamentLobby -= OnJoinTournamentLobbyAttempted;
        _myPhotonCallbacks._OnJoinedLobby -= OnJoinedLobby;
        _myPhotonCallbacks._OnLeftLobby -= OnLeftLobby;
    }

    private void OnJoinTournamentLobbyAttempted(TitleGroupProperties titleGroupProperties)
    {
        _groupId = titleGroupProperties.GroupID;
        _groupType = titleGroupProperties.GroupType;

        TitleGroupProperties newTitleGroupProperties = new TitleGroupProperties(_groupId, _groupType, Data.Manager.EntityID, Data.Manager.EntityType);

        ExternalData.TitleGroups.RequestToBecomeMember(newTitleGroupProperties,
            isMemeber =>
            {
                if (isMemeber)
                {
                    JoinTournamentLobby();
                    GlobalFunctions.DebugLog("Is a member");
                }
                else
                {
                    ExternalData.TitleGroups.Apply(newTitleGroupProperties, onResult =>
                    {
                        if (onResult)
                        {
                            JoinTournamentLobby();
                        }
                    });
                }
            });
    }

    private void JoinTournamentLobby()
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, true);
        MyPhoton.JoinLobby(_groupId, LobbyType.Default);
    }

    private void OnJoinedLobby()
    {
        if (IsCorrectLobby)
        {
            onTournamentLobbyJoined?.Invoke(new TitleGroupProperties(_groupId, _groupType, null, null));
            GlobalFunctions.DebugLog("Joined " + MyPhotonNetwork.CurrentLobby.Name + "/" + MyPhotonNetwork.IsInRoom);
        }
    }

    private void OnLeftLobby()
    {
        if (IsCorrectLobby)
        {
            GlobalFunctions.DebugLog("Joined " + MyPhotonNetwork.CurrentLobby.Name + "/" + MyPhotonNetwork.IsInRoom);
        }
    }
}
