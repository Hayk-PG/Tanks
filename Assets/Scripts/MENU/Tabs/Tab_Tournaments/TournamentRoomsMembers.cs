using PlayFab.GroupsModels;
using PlayFab.ProfilesModels;
using System;
using System.Collections.Generic;
using UnityEngine;


public class TournamentRoomsMembers : MonoBehaviour
{
    private Tab_TournamentLobby _tabTournamentLobby;
    private TitleProperties _titleProperties;

    private string _memberRoleId = "members";

    public event Action<TournamentMemberPublicData> onShareTournamentRoomsMembers;


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
            GetMembersPlayfabID(objectData, entityWithLineage);
        }
    }

    private void GetMembersPlayfabID(Dictionary<string, object> objectData, EntityWithLineage entityWithLineage)
    {
        ExternalData.Entity.GetPlayerProfileFromEntity(entityWithLineage.Key.Id, entityWithLineage.Key.Type, result => { GetMembersProfile(objectData, result); });
    }

    private void GetMembersProfile(Dictionary<string, object> objectData, GetEntityProfileResponse getEntityProfileResponse)
    {
        ExternalData.Profile.Get(getEntityProfileResponse.Profile.Lineage.MasterPlayerAccountId, result =>
        {
            onShareTournamentRoomsMembers?.Invoke(new TournamentMemberPublicData { MemberName = result.PlayerProfile.DisplayName, MemberPlayfabID = getEntityProfileResponse.Profile.Lineage.MasterPlayerAccountId, MemberRoomName = (string)objectData[TournamentObjectData.KeyRoomName] });
            GlobalFunctions.DebugLog(result.PlayerProfile.DisplayName + "/" + (string)objectData[TournamentObjectData.KeyRoomName]);
        });
    }
}
