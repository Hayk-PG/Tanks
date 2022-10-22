using PlayFab.GroupsModels;
using System;
using UnityEngine;

public class TournamentMembers : MonoBehaviour
{
    private Tab_TournamentLobby _tournamentLobby;

    private string _memberRoleId = "members";

    public event Action<TitleGroupProperties, int> onShareProperties;


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

    private void ListTitleGroupMembers(TitleGroupProperties titleGroupProperties)
    {
        print(MyPhotonNetwork.CurrentLobby.Name);
        //ExternalData.TitleGroups.ListMembers(titleGroupProperties,
        //    result =>
        //    {
        //        if (result != null)
        //        {
        //            GetTitleGroupMembers(result);
        //        }
        //        else
        //        {
        //            GlobalFunctions.DebugLog("No members");
        //        }
        //    });
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
        for (int i = 0; i < member.Members.Count; i++)
        {
            onShareProperties?.Invoke(new TitleGroupProperties(null, null, member.Members[i].Key.Id, member.Members[i].Key.Type), i);
        }
    }
}
