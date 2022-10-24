using PlayFab.GroupsModels;
using PlayFab.ProfilesModels;
using System.Collections.Generic;
using UnityEngine;

public class DisplayTournamentRoomsMembers : MonoBehaviour
{
    [SerializeField] private TournamentRoom[] _tournamentRooms;

    private Tab_TournamentLobby _tabTournamentLobby;
    private TitleProperties _titleProperties;

    private string _memberRoleId = "members";


    private void Awake()
    {
        _tabTournamentLobby = Get<Tab_TournamentLobby>.From(gameObject);
    }

    private void OnEnable()
    {
        _tabTournamentLobby.onTournamentLobbyJoined += JoinedTournamentLobby;
    }

    private void OnDisable()
    {
        _tabTournamentLobby.onTournamentLobbyJoined -= JoinedTournamentLobby;
    }

    private void JoinedTournamentLobby(TitleProperties titleProperties)
    {
        if (titleProperties == null)
            return;

        _titleProperties = titleProperties;

        ExternalData.TitleGroups.ListMembers(_titleProperties, GetMembers);
    }

    private void GetMembers(EntityMemberRole entityMemberRole, EntityWithLineage entityWithLineage)
    {
        ExternalData.EntityObjects.Get(new TitleProperties(null, null, entityWithLineage.Key.Id, entityWithLineage.Key.Type), TournamentObjectData.ObjectName, result => { GetMembersObject(result, entityMemberRole, entityWithLineage); });
    }

    private void GetMembersObject(Dictionary<string, object> objectData, EntityMemberRole entityMemberRole, EntityWithLineage entityWithLineage)
    {
        if(objectData.ContainsKey(TournamentObjectData.KeyRoomName)/* && (string)objectData[TournamentObjectData.KeyRoomName] != TournamentObjectData.ValueNotSet*/)
        {
            GetMembersPlayfabID(entityWithLineage);
            print((string)objectData[TournamentObjectData.KeyRoomName]);
        }
    }

    private void GetMembersPlayfabID(EntityWithLineage entityWithLineage)
    {
        ExternalData.Entity.GetPlayerProfileFromEntity(entityWithLineage.Key.Id, entityWithLineage.Key.Type, GetMembersProfile);
    }

    private void GetMembersProfile(GetEntityProfileResponse getEntityProfileResponse)
    {
        ExternalData.Profile.Get(getEntityProfileResponse.Profile.Lineage.MasterPlayerAccountId, result =>
        {
            print(result.PlayerProfile.DisplayName);
        });
    }
}
