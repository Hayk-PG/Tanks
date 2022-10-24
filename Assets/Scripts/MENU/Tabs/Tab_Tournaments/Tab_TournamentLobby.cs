using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;

public class Tab_TournamentLobby : MonoBehaviourPun
{
    private CanvasGroup _canvasGroup;
    private TitleGroupTournament _titleGroupTournament;
    private MyPhotonCallbacks _myPhotonCallbacks;
    private PreciseNetworkClientCallbacks _preciseNetworkClientCallbacks;

    private TitleProperties _titleProperties;

    private bool IsCorrectLobby
    {
        get => _titleProperties != null && !String.IsNullOrEmpty(_titleProperties.GroupID) && MyPhotonNetwork.CurrentLobby.Name == _titleProperties.GroupID;
    }

    public event Action<TitleProperties> onTournamentLobbyJoined;


    private void Awake()
    {
        _canvasGroup = Get<CanvasGroup>.From(gameObject);
        _titleGroupTournament = FindObjectOfType<TitleGroupTournament>();
        _myPhotonCallbacks = FindObjectOfType<MyPhotonCallbacks>();
        _preciseNetworkClientCallbacks = FindObjectOfType<PreciseNetworkClientCallbacks>();
    }

    private void OnEnable()
    {
        _titleGroupTournament.onAttemptToJoinTournamentLobby += OnJoinTournamentLobbyAttempted;
        _myPhotonCallbacks._OnJoinedLobby += OnJoinedLobby;
        _preciseNetworkClientCallbacks.onNetworkClientState += PreciseNetworkClientStateReceived;
    }

    private void OnDisable()
    {
        _titleGroupTournament.onAttemptToJoinTournamentLobby -= OnJoinTournamentLobbyAttempted;
        _myPhotonCallbacks._OnJoinedLobby -= OnJoinedLobby;
        _preciseNetworkClientCallbacks.onNetworkClientState -= PreciseNetworkClientStateReceived;
    }

    private void CanvasGroupActivity(bool isActive)
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, isActive); 
    }

    private void DeleteTournamentDataFromObject(bool isTournamentClosed)
    {
        if (isTournamentClosed)
            ExternalData.EntityObjects.Delete(new TitleProperties(null, null, Data.Manager.EntityID, Data.Manager.EntityType), TournamentObjectData.ObjectName, result => { });
    }

    private void OnJoinTournamentLobbyAttempted(TitleProperties titleProperties)
    {
        _titleProperties = new TitleProperties(titleProperties.GroupID, titleProperties.GroupType, Data.Manager.EntityID, Data.Manager.EntityType);

        ExternalData.TitleGroups.RequestToBecomeMember(_titleProperties,
            isMemeber =>
            {
                if (isMemeber)
                {
                    JoinTournamentLobby();
                    GlobalFunctions.DebugLog("Is a member");
                }
                else
                {
                    ExternalData.TitleGroups.Apply(_titleProperties, onResult =>
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
        MyPhoton.JoinLobby(_titleProperties.GroupID, LobbyType.Default);
        CanvasGroupActivity(false);
    }

    private void OnJoinedLobby()
    {
        onTournamentLobbyJoined?.Invoke(IsCorrectLobby ? _titleProperties : null);
        CanvasGroupActivity(IsCorrectLobby);
    }

    private void PreciseNetworkClientStateReceived(ClientState previousState, ClientState currentState)
    {
        bool wasJoinedPreviously = previousState == ClientState.Joined || previousState == ClientState.JoinedLobby || previousState == ClientState.Joining || previousState == ClientState.JoiningLobby || previousState == ClientState.Leaving;
        bool hasLeft = currentState != ClientState.Joined && currentState != ClientState.JoinedLobby && currentState != ClientState.Joining && currentState != ClientState.JoiningLobby && currentState != ClientState.Leaving;

        if (wasJoinedPreviously && hasLeft)
        {
            CanvasGroupActivity(false);
            DeleteTournamentDataFromObject(true);
            GlobalFunctions.DebugLog(previousState + "/" + currentState);
        }
    }
}
