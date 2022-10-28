using Photon.Realtime;
using System;

public class Tab_TournamentLobby : Tab_Base<TitleGroupTournament>
{
    private MyPhotonCallbacks _myPhotonCallbacks;
    private TitleProperties _titleProperties;

    private bool _isInTournamentLobby;
    private bool IsCorrectLobby
    {
        get => _titleProperties != null && !String.IsNullOrEmpty(_titleProperties.GroupID) && MyPhotonNetwork.CurrentLobby.Name == _titleProperties.GroupID;
    }

    public event Action<TitleProperties> onTournamentLobbyJoined;
    public event Action onCloseTournamentLobby;


    protected override void Awake()
    {
        base.Awake();

        _myPhotonCallbacks = FindObjectOfType<MyPhotonCallbacks>();
    }

    private void OnEnable()
    {
        _object.onClickTournamentLobbyButton += AttemptToJoinLobby;
        _myPhotonCallbacks._OnJoinedLobby += OnJoinedLobby;
        _myPhotonCallbacks._OnLeftLobby += CloseTournamentLobby;
        _myPhotonCallbacks._OnJoinedRoom += delegate { CloseTournamentLobby(); };
        _myPhotonCallbacks.onDisconnect += delegate { CloseTournamentLobby(); };
    }

    private void OnDisable()
    {
        _object.onClickTournamentLobbyButton -= AttemptToJoinLobby;
        _myPhotonCallbacks._OnJoinedLobby -= OnJoinedLobby;
        _myPhotonCallbacks._OnLeftLobby -= CloseTournamentLobby;
        _myPhotonCallbacks._OnJoinedRoom -= delegate { CloseTournamentLobby(); };
        _myPhotonCallbacks.onDisconnect -= delegate { CloseTournamentLobby(); };
    }

    private void CanvasGroupActivity(bool isActive)
    {
        GlobalFunctions.CanvasGroupActivity(CanvasGroup, isActive); 
    }

    private void AttemptToJoinLobby(TitleProperties titleProperties)
    {
        _titleProperties = new TitleProperties(titleProperties.GroupID, titleProperties.GroupType, Data.Manager.EntityID, Data.Manager.EntityType);

        ExternalData.TitleGroups.RequestToBecomeMember(_titleProperties, isMemeber => { JoinTournamentLobbyThroughTitleGroup(isMemeber); });
    }

    private void JoinTournamentLobbyThroughTitleGroup(bool isGroupMember)
    {
        if(isGroupMember)
            JoinTournamentLobby();
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
    }

    private void JoinTournamentLobby()
    {
        MyPhoton.JoinLobby(_titleProperties.GroupID, LobbyType.SqlLobby);
        CanvasGroupActivity(false);
        _isInTournamentLobby = true;
    }

    private void OnJoinedLobby()
    {
        if (IsCorrectLobby == false)
        {
            return;
        }

        onTournamentLobbyJoined?.Invoke(_titleProperties);
        CanvasGroupActivity(true);
    }

    private void CloseTournamentLobby()
    {
        onCloseTournamentLobby?.Invoke();
    }

    private void CanselGroupSubscription()
    {
        if (_isInTournamentLobby)
        {
            ExternalData.TitleGroups.RemoveMember(new TitleProperties(_titleProperties.GroupID, _titleProperties.GroupType, Data.Manager.EntityID, Data.Manager.EntityType), null);
            ExternalData.EntityObjects.Delete(new TitleProperties(null, null, Data.Manager.EntityID, Data.Manager.EntityType), TournamentObjectData.ObjectName, null);
            GlobalFunctions.DebugLog("Exit tournament group");
            _isInTournamentLobby = false;
        }
    }
}
