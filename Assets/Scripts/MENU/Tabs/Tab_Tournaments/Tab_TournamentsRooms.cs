using UnityEngine;

public class Tab_TournamentsRooms : MonoBehaviour
{   
    private TournamentMembers _tournamentMembers;
    private Tab_TournamentLobby _tabTournamentLobby;
    private TitleProperties _titleProperties;
    private MyPhotonCallbacks _myPhotonCallbacks;

    public TournamentRoomButton[] TournamentRoomButtons { get; private set; }


    private void Awake()
    {       
        _tournamentMembers = Get<TournamentMembers>.From(gameObject);
        _tabTournamentLobby = FindObjectOfType<Tab_TournamentLobby>();
        _myPhotonCallbacks = FindObjectOfType<MyPhotonCallbacks>();

        TournamentRoomButtons = GetComponentsInChildren<TournamentRoomButton>();
    }

    private void OnEnable()
    {
        _tournamentMembers.onShareProperties += ReceiveMembesrProperties;
        _tabTournamentLobby.onTournamentLobbyJoined += JoinedTournamentLobby;
        _myPhotonCallbacks._OnJoinedRoom += JoinedRoom;

        GlobalFunctions.Loop<TournamentRoomButton>.Foreach(TournamentRoomButtons, tournamentRoomButton => 
        {
            tournamentRoomButton.onClickTournamentRoomButton += TournamentRoomButtonClicked;
        });
    }

    private void OnDisable()
    {
        _tournamentMembers.onShareProperties -= ReceiveMembesrProperties;
        _tabTournamentLobby.onTournamentLobbyJoined -= JoinedTournamentLobby;
        _myPhotonCallbacks._OnJoinedRoom -= JoinedRoom;

        GlobalFunctions.Loop<TournamentRoomButton>.Foreach(TournamentRoomButtons, tournamentRoomButton =>
        {
            tournamentRoomButton.onClickTournamentRoomButton -= TournamentRoomButtonClicked;
        });
    }

    private void ReceiveMembesrProperties(TitleProperties titleProperties)
    {
        
    }

    private void CacheTitleProperties(TitleProperties titleProperties)
    {
        _titleProperties = titleProperties;
    }

    private void JoinedTournamentLobby(TitleProperties titleProperties)
    {
        CacheTitleProperties(titleProperties);

        TitleProperties newTitleGroupProperties = _titleProperties != null ? _titleProperties : new TitleProperties(null, null, Data.Manager.EntityID, Data.Manager.EntityType);

        TournamentObjectValues tournamentObjectValues = new TournamentObjectValues
            (
            _titleProperties != null ? TournamentObjectData.ValueActive : TournamentObjectData.ValuePassive,
            _titleProperties != null ? titleProperties.GroupID : TournamentObjectData.ValueNotSet,
            TournamentObjectData.ValuePassive,
            TournamentObjectData.ValueNotSet,
            TournamentObjectData.ValueNotSet
            );

        ExternalData.EntityObjects.Set(newTitleGroupProperties, TournamentObjectData.ObjectName, TournamentObjectData.ObjectData(TournamentObjectValues.DataObjectValues(tournamentObjectValues)), result => { });
    }

    private void TournamentRoomButtonClicked(TournamentRoom tournamentRoom)
    {
        if (_titleProperties == null)
            return;

        ExternalData.EntityObjects.CheckEntityObject(_titleProperties, TournamentObjectData.ObjectName, TournamentObjectData.KeyTournamentName, _titleProperties.GroupID, result => 
        {
            MyPhoton.CreateRoom(Photon.Realtime.LobbyType.Default, _titleProperties.GroupID, tournamentRoom.RoomName, null, false, 0, 30, false);
        });
    }

    private void JoinedRoom(Photon.Realtime.Room room)
    {
        if (_titleProperties == null)
            return;

        ExternalData.EntityObjects.CheckEntityObject(_titleProperties, TournamentObjectData.ObjectName, TournamentObjectData.KeyTournamentStatus, TournamentObjectData.ValueActive, result =>
        {
            TournamentObjectValues tournamentObjectValues = new TournamentObjectValues
            (
                TournamentObjectData.ValuePassive,
                _titleProperties.GroupID,
                TournamentObjectData.ValueActive,
                room.Name,
                TournamentObjectData.ValueNotSet
            );

            ExternalData.EntityObjects.Set(_titleProperties, TournamentObjectData.ObjectName, TournamentObjectData.ObjectData(TournamentObjectValues.DataObjectValues(tournamentObjectValues)), result => { });
        });
    }
}
