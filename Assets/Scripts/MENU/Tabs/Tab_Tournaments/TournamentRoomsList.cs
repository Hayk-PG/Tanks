using System;
using UnityEngine;

public class TournamentRoomsList : MonoBehaviour
{
    [SerializeField] private TournamentRoom[] _tournamentRooms;

    private TournamentRoomsMembers _tournamentRoomsMembers;


    private void Awake()
    {
        _tournamentRoomsMembers = Get<TournamentRoomsMembers>.From(gameObject);
    }

    private void OnEnable()
    {
        _tournamentRoomsMembers.onShareTournamentRoomsMembers += GetTournamentRoomsMembers;    
    }

    private void OnDisable()
    {
        _tournamentRoomsMembers.onShareTournamentRoomsMembers -= GetTournamentRoomsMembers;
    }

    private void GetTournamentRoomsMembers(TournamentMemberPublicData tournamentMemberPublicData)
    {
        GlobalFunctions.Loop<TournamentRoom>.Foreach(_tournamentRooms, tournamentRoom => { UpdateRoomsDisplay(tournamentRoom, tournamentMemberPublicData); });
    }

    private void UpdateRoomsDisplay(TournamentRoom tournamentRoom, TournamentMemberPublicData tournamentMemberPublicData)
    {
        if(tournamentRoom.RoomName == tournamentMemberPublicData._memberRoomName)
        {
            if (String.IsNullOrEmpty(tournamentRoom.MasterOpponentName))
                tournamentRoom.SetData(new TournamentRoomData { MasterOpponentName = tournamentMemberPublicData._memberName });

            else
                tournamentRoom.SetData(new TournamentRoomData { SecondOpponentName = tournamentMemberPublicData._memberName });
        }
    }

    //REMOVE NON MEMBER'S NAME FROM ROOMS
}
