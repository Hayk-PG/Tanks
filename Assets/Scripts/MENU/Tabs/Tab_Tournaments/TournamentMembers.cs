using PlayFab.GroupsModels;
using System;
using UnityEngine;

public class TournamentMembers : MonoBehaviour
{
    private Tab_TournamentLobby _tournamentLobby;

    private string _memberRoleId = "members";

    public event Action<TitleProperties> onShareProperties;


    private void Awake()
    {
        _tournamentLobby = Get<Tab_TournamentLobby>.From(gameObject);
    }

    private void OnEnable()
    {
        _tournamentLobby.onTournamentLobbyJoined += ListTitleGroupMembers;
    }

    private void OnDisable()
    {
        _tournamentLobby.onTournamentLobbyJoined -= ListTitleGroupMembers;
    }

    private void ListTitleGroupMembers(TitleProperties titleGroupProperties)
    {
        if (titleGroupProperties == null)
            return;

        ExternalData.TitleGroups.ListMembers(titleGroupProperties,
            result =>
            {
                if (result != null)
                {
                    GetTitleGroupMembers(result);
                }
                else
                {
                    GlobalFunctions.DebugLog("No members");
                }
            });
    }

    private void GetTitleGroupMembers(ListGroupMembersResponse listGroupMembersResponse)
    {
        GlobalFunctions.Loop<EntityMemberRole>.Foreach(listGroupMembersResponse.Members, 
            member => 
            { 
                if(member.RoleId == _memberRoleId)
                {
                    GetTitleGroupMembersInfo(member);
                }
            });
    }

    private void GetTitleGroupMembersInfo(EntityMemberRole member)
    {
        GlobalFunctions.Loop<EntityWithLineage>.Foreach(member.Members,
            memberInfo =>
            {
                onShareProperties?.Invoke(new TitleProperties(null, null, memberInfo.Key.Id, memberInfo.Key.Type));
            });
    }
}
