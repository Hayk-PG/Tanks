using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TournamentRoomsList : MonoBehaviourPunCallbacks
{
    [SerializeField] private List<TournamentRoom> _tournamentRooms;

    private List<string> _friendsList = new List<string>();
    private Dictionary<string, string> _membersInRoom = new Dictionary<string, string>();

    private TournamentRoomsMembers _tournamentRoomsMembers;


    private void Awake()
    {
        _tournamentRoomsMembers = Get<TournamentRoomsMembers>.From(gameObject);
    }

    private void Start()
    {
        _tournamentRoomsMembers.onShareTournamentRoomsMembers += GetTournamentRoomsMembers;    
    }

    private void OnDestroy()
    {
        _tournamentRoomsMembers.onShareTournamentRoomsMembers -= GetTournamentRoomsMembers;
    }

    private void GetTournamentRoomsMembers(TournamentMemberPublicData tournamentMemberPublicData)
    {
        if(_friendsList.Find(userid => userid == tournamentMemberPublicData.MemberPlayfabID) == null)
        {
            _friendsList.Add(tournamentMemberPublicData.MemberPlayfabID);
        }

        PhotonNetwork.FindFriends(_friendsList.ToArray());
    }

    private void FixedUpdate()
    {
        foreach (var member in _membersInRoom)
        {
            TournamentRoom room = _tournamentRooms.Find(room => room.RoomName == member.Value);

            if(room.MasterPlayerID == member.Key || room.SecondPlayerID == member.Key)
            {
                return;
            }

            SetPlayerName(room, member);
        }
    }

    private void SetPlayerName(TournamentRoom room, KeyValuePair<string, string> member)
    {
        if (String.IsNullOrEmpty(room.MasterPlayerID))
        {
            room.SetPlayerName(new TournamentRoomData { MasterPlayerID = member.Key });
        }
        else
        {
            room.SetPlayerName(new TournamentRoomData { SecondPlayerID = member.Key });
        }
    }

    public override void OnFriendListUpdate(List<FriendInfo> friendList)
    {
        foreach (var friend in friendList)
        {
            if (friend.IsInRoom)
            {
                AddMember(friend);
            }
            else
            {
                RemoveMember(friend);
            }
        }
    }

    private void AddMember(FriendInfo friendInfo)
    {
        if (!_membersInRoom.ContainsKey(friendInfo.UserId))
        {
            _membersInRoom.Add(friendInfo.UserId, friendInfo.Room);
        }
        else
        {
            _membersInRoom[friendInfo.UserId] = friendInfo.Room;
        }
    }

    private void RemoveMember(FriendInfo friendInfo)
    {
        if (_membersInRoom.ContainsKey(friendInfo.UserId))
        {
            _membersInRoom.Remove(friendInfo.UserId);

            foreach (var room in _tournamentRooms)
            {
                if (room.MasterPlayerID == friendInfo.UserId)
                    room.DeleteMasterPlayerName();

                if (room.SecondPlayerID == friendInfo.UserId)
                    room.DeleteSecondPlayerName();
            }
        }
    }
}
