using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;

public class Tab_TournamentLobby : MonoBehaviourPun
{
    private CanvasGroup _canvasGroup;
    private TitleGroupTournament _titleGroupTournament;
    private MyPhotonCallbacks _myPhotonCallbacks;
    private TitleProperties _titleGroupProperties;

    private bool IsCorrectLobby
    {
        get => _titleGroupProperties != null && !String.IsNullOrEmpty(_titleGroupProperties.GroupID) && MyPhotonNetwork.CurrentLobby.Name == _titleGroupProperties.GroupID;
    }

    public event Action<TitleProperties> onTournamentLobbyJoined;


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

    private void OnJoinTournamentLobbyAttempted(TitleProperties titleGroupProperties)
    {
        _titleGroupProperties = new TitleProperties(titleGroupProperties.GroupID, titleGroupProperties.GroupType, Data.Manager.EntityID, Data.Manager.EntityType);

        ExternalData.TitleGroups.RequestToBecomeMember(_titleGroupProperties,
            isMemeber =>
            {
                if (isMemeber)
                {
                    JoinTournamentLobby();
                    GlobalFunctions.DebugLog("Is a member");
                }
                else
                {
                    ExternalData.TitleGroups.Apply(_titleGroupProperties, onResult =>
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
        MyPhoton.JoinLobby(_titleGroupProperties.GroupID, LobbyType.Default);
    }

    private void OnJoinedLobby()
    {
        if (IsCorrectLobby)
        {
            onTournamentLobbyJoined?.Invoke(_titleGroupProperties);
        }
        else
        {
            onTournamentLobbyJoined?.Invoke(null);
        }

        GlobalFunctions.DebugLog("Joined " + MyPhotonNetwork.CurrentLobby.Name + "/" + MyPhotonNetwork.IsInRoom);
    }

    private void OnLeftLobby()
    {
        if (IsCorrectLobby)
        {
            GlobalFunctions.DebugLog("Joined " + MyPhotonNetwork.CurrentLobby.Name + "/" + MyPhotonNetwork.IsInRoom);
        }
    }
}
