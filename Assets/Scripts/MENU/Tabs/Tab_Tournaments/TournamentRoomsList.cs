using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TournamentRoomsList : MonoBehaviourPunCallbacks
{
    [SerializeField] private TournamentRoom[] _tournamentRooms;
    [SerializeField] private bool _execute;

    private List<string> _friendsList = new List<string>();

    private TournamentRoomsMembers _tournamentRoomsMembers;


    private void Awake()
    {
        _tournamentRoomsMembers = Get<TournamentRoomsMembers>.From(gameObject);
    }

    private void Start()
    {
        _tournamentRoomsMembers.onShareTournamentRoomsMembers += GetTournamentRoomsMembers;    
    }

    private void Update()
    {
        if (_execute)
        {
            if (_friendsList.Find(userid => userid == "3E76577479D0A4D1") == null)
            {
                _friendsList.Add("3E76577479D0A4D1");
            }

            if (PhotonNetwork.FindFriends(_friendsList.ToArray()))
            {
                GlobalFunctions.DebugLog("Found friend");
            }

            _execute = false;
        }
    }

    //private void OnDisable()
    //{
    //    _tournamentRoomsMembers.onShareTournamentRoomsMembers -= GetTournamentRoomsMembers;
    //}

    private void GetTournamentRoomsMembers(TournamentMemberPublicData tournamentMemberPublicData)
    {
        GlobalFunctions.DebugLog(tournamentMemberPublicData.MemberName);

        if(_friendsList.Find(userid => userid == tournamentMemberPublicData.MemberPlayfabID) == null)
        {
            _friendsList.Add(tournamentMemberPublicData.MemberPlayfabID);
        }

        if (PhotonNetwork.FindFriends(_friendsList.ToArray()))
        {
            GlobalFunctions.DebugLog("Found friend");
        }
    }

    private void UpdateRoomsDisplay(TournamentRoom tournamentRoom, TournamentMemberPublicData tournamentMemberPublicData)
    {
        if(tournamentRoom.RoomName == tournamentMemberPublicData.MemberRoomName)
        {
            if (String.IsNullOrEmpty(tournamentRoom.MasterOpponentName))
                tournamentRoom.SetData(new TournamentRoomData { MasterOpponentName = tournamentMemberPublicData.MemberName });

            else
                tournamentRoom.SetData(new TournamentRoomData { SecondOpponentName = tournamentMemberPublicData.MemberName });
        }
    }

    public override void OnFriendListUpdate(List<FriendInfo> friendList)
    {
        foreach (var friend in friendList)
        {
            GlobalFunctions.DebugLog(friend.UserId + "/" + friend.IsOnline + "/" + friend.IsInRoom);
        }
    }

    //REMOVE NON MEMBER'S NAME FROM ROOMS
}
