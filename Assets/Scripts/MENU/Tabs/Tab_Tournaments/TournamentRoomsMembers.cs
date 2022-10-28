using PlayFab.GroupsModels;
using PlayFab.ProfilesModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TournamentRoomsMembers : MonoBehaviour
{
    private Tab_TournamentLobby _tabTournamentLobby;
    private TitleProperties _titleProperties;

    private string _memberRoleId = "members";

    private bool _hasMemberProfileGotten;
    private bool _isCoroutineRunning;

    private IEnumerator _coroutine;

    public event Action<TournamentMemberPublicData> onShareTournamentRoomsMembers;


    private void Awake()
    {
        _tabTournamentLobby = Get<Tab_TournamentLobby>.From(gameObject);
    }

    private void OnEnable()
    {
        _tabTournamentLobby.onTournamentLobbyJoined += JoinedTournamentLobby;
        _tabTournamentLobby.onCloseTournamentLobby += StopEverything;
    }

    private void OnDisable()
    {
        _tabTournamentLobby.onTournamentLobbyJoined -= JoinedTournamentLobby;
        _tabTournamentLobby.onCloseTournamentLobby -= StopEverything;
    }

    private void JoinedTournamentLobby(TitleProperties titleProperties)
    {
        if (titleProperties == null)
            return;

        _titleProperties = titleProperties;

        StartMembersListCoroutine();
    }

    private void StartMembersListCoroutine()
    {
        _isCoroutineRunning = false;

        if (_coroutine == null)
        {
            _isCoroutineRunning = true;
            _coroutine = RunMembersList();            
            StartCoroutine(_coroutine);
            GlobalFunctions.DebugLog("Coroutine is started");
        }        
    }

    private void StopEverything()
    {
        if (_coroutine != null)
        {
            _isCoroutineRunning = false;
            StopCoroutine(_coroutine);
            _coroutine = null;
            GlobalFunctions.DebugLog("Coroutine has been stopped");
        }
    }

    private IEnumerator RunMembersList()
    {
        while (_isCoroutineRunning)
        {
            _hasMemberProfileGotten = false;
            ExternalData.TitleGroups.ListMembers(_titleProperties, GetMembers);
            yield return new WaitUntil(()=> _hasMemberProfileGotten || !_isCoroutineRunning);
            yield return new WaitForSeconds(1);
        }
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
            if (result == null)
                return;

            onShareTournamentRoomsMembers?.Invoke(new TournamentMemberPublicData { MemberName = result.PlayerProfile.DisplayName, MemberPlayfabID = getEntityProfileResponse.Profile.Lineage.MasterPlayerAccountId, MemberRoomName = (string)objectData[TournamentObjectData.KeyRoomName] });
            _hasMemberProfileGotten = true;
        });
    }
}
