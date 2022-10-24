using UnityEngine;
using Photon.Realtime;
using System.Collections.Generic;

public class Tab_TournamentsRooms : MonoBehaviour
{   
    private Tab_TournamentLobby _tabTournamentLobby;
    private TitleProperties _titleProperties;
    private MyPhotonCallbacks _myPhotonCallbacks;  

    /// <summary>
    /// 0:TournamentStatus, 1:TournamentName, 2:RoomStatus, 3:RoomName, 4:GameStatus
    /// </summary>
    private object[] _dataObject = new object[5] { TournamentObjectData.ValueNotSet, TournamentObjectData.ValueNotSet, TournamentObjectData.ValueNotSet, TournamentObjectData.ValueNotSet, TournamentObjectData.ValueNotSet };

    public TournamentRoomButton[] TournamentRoomButtons { get; private set; }


    private void Awake()
    {       
        _tabTournamentLobby = FindObjectOfType<Tab_TournamentLobby>();
        _myPhotonCallbacks = FindObjectOfType<MyPhotonCallbacks>();      

        TournamentRoomButtons = GetComponentsInChildren<TournamentRoomButton>();
    }

    private void OnEnable()
    {
        _tabTournamentLobby.onTournamentLobbyJoined += JoinedTournamentLobby;
        _myPhotonCallbacks._OnJoinedRoom += JoinedRoom;      

        GlobalFunctions.Loop<TournamentRoomButton>.Foreach(TournamentRoomButtons, tournamentRoomButton => 
        {
            tournamentRoomButton.onClickTournamentRoomButton += TournamentRoomButtonClicked;
        });
    }

    private void OnDisable()
    {
        _tabTournamentLobby.onTournamentLobbyJoined -= JoinedTournamentLobby;
        _myPhotonCallbacks._OnJoinedRoom -= JoinedRoom;        

        GlobalFunctions.Loop<TournamentRoomButton>.Foreach(TournamentRoomButtons, tournamentRoomButton =>
        {
            tournamentRoomButton.onClickTournamentRoomButton -= TournamentRoomButtonClicked;
        });
    }

    private void JoinedTournamentLobby(TitleProperties titleProperties)
    {
        _titleProperties = titleProperties;

        if (_titleProperties != null)
        {
            _dataObject[0] = TournamentObjectData.ValueActive;
            _dataObject[1] = titleProperties.GroupID;
            _dataObject[2] = TournamentObjectData.ValuePassive;
            _dataObject[3] = TournamentObjectData.ValueNotSet;
            _dataObject[4] = TournamentObjectData.ValueNotSet;

            ExternalData.EntityObjects.Set(_titleProperties, TournamentObjectData.ObjectName, TournamentObjectData.ObjectData(_dataObject), result => { });
        }
        else
        {
            GlobalFunctions.DebugLog("Joined unnamed lobby");
        }
    }

    private void TournamentRoomButtonClicked(TournamentRoom tournamentRoom)
    {
        if (_titleProperties == null)
            return;

        ExternalData.EntityObjects.Get(_titleProperties, TournamentObjectData.ObjectName, result => { CreateRoom(result, tournamentRoom); });
    }

    private void CreateRoom(Dictionary<string, object> dataObject, TournamentRoom tournamentRoom)
    {
        if (dataObject.ContainsKey(TournamentObjectData.KeyTournamentName) && (string)dataObject[TournamentObjectData.KeyTournamentName] == _titleProperties.GroupID)
            MyPhoton.CreateRoom(LobbyType.Default, _titleProperties.GroupID, tournamentRoom.RoomName, null, false, 0, 30, false);
    }

    private void JoinedRoom(Room room)
    {
        if (_titleProperties == null)
            return;

        ExternalData.EntityObjects.Get(_titleProperties, TournamentObjectData.ObjectName, result => { UpdateEntityObjectOnJoinedRoom(result, room); });
    }

    private void UpdateEntityObjectOnJoinedRoom(Dictionary<string, object> dataObject, Room room)
    {
        if(dataObject.ContainsKey(TournamentObjectData.KeyTournamentStatus) && (string)dataObject[TournamentObjectData.KeyTournamentStatus] == TournamentObjectData.ValueActive)
        {
            _dataObject[0] = TournamentObjectData.ValuePassive;
            _dataObject[2] = TournamentObjectData.ValueActive;
            _dataObject[3] = room.Name;

            ExternalData.EntityObjects.Set(_titleProperties, TournamentObjectData.ObjectName, TournamentObjectData.ObjectData(_dataObject), result => { });
        }
    }
}
