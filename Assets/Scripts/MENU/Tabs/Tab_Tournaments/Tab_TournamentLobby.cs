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

    private bool _isInTournamentLobby;
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
        MyPhoton.JoinLobby(_titleProperties.GroupID, LobbyType.SqlLobby);
        CanvasGroupActivity(false);
        _isInTournamentLobby = true;
    }

    private void OnJoinedLobby()
    {
        onTournamentLobbyJoined?.Invoke(IsCorrectLobby ? _titleProperties : null);
        CanvasGroupActivity(IsCorrectLobby);
    }

    private void PreciseNetworkClientStateReceived(ClientState previousState, ClientState currentState)
    {
        bool hasLeft = currentState != ClientState.Joined && currentState != ClientState.JoinedLobby && currentState != ClientState.Joining && currentState != ClientState.JoiningLobby;

        if (hasLeft)
        {
            ExitTournamentGroup();
        }
    }

    private void ExitTournamentGroup()
    {
        //if (_isInTournamentLobby)
        //{
        //    ExternalData.TitleGroups.RemoveMember(new TitleProperties(_titleProperties.GroupID, _titleProperties.GroupType, Data.Manager.EntityID, Data.Manager.EntityType), null);
        //    ExternalData.EntityObjects.Delete(new TitleProperties(null, null, Data.Manager.EntityID, Data.Manager.EntityType), TournamentObjectData.ObjectName, null);
        //    GlobalFunctions.DebugLog("Exit tournament group");
        //    _isInTournamentLobby = false;
        //}
    }
}
