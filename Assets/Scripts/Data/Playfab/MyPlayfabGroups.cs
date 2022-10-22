using UnityEngine;
using PlayFab;
using PlayFab.GroupsModels;

public class MyPlayfabGroups : MonoBehaviour
{
    private string[] _groupsId;
    private string _groupsType;

    [SerializeField] 
    private bool _execute;

    [SerializeField]
    private bool _shareGroupsEntityKeys;


    private void ListMembers()
    {
        ListGroupMembersRequest listGroupMembersRequest = new ListGroupMembersRequest();
        listGroupMembersRequest.Group = new PlayFab.GroupsModels.EntityKey() { Id = "CBE02DC5CDA093FF", Type = "title_player_account" };

        PlayFabGroupsAPI.ListGroupMembers(listGroupMembersRequest, 
            s =>
            {
                foreach (var item in s.Members)
                {
                    foreach(var member in item.Members)
                    {
                        print(member.Key.Id + "/" + item.RoleName);
                    }
                }
            } , 
            e => 
            {
                print(e.ErrorMessage);
            });
    }

    private void Test2()
    {
        ApplyToGroupRequest applyToGroupRequest = new ApplyToGroupRequest();
        applyToGroupRequest.Entity = new PlayFab.GroupsModels.EntityKey() { Id = "C28C53A93DE90602", Type = "title_player_account" };
        applyToGroupRequest.Group = new PlayFab.GroupsModels.EntityKey() { Id = "CBE02DC5CDA093FF", Type = "title_player_account" };

        PlayFabGroupsAPI.ApplyToGroup(applyToGroupRequest,
            onSucces =>
            {
                Test4(applyToGroupRequest.Entity);
            },
            onError =>
            {
                print(onError.ErrorMessage);
            });
    }

    private void Test3()
    {
        InviteToGroupRequest inviteToGroupRequest = new InviteToGroupRequest();
        inviteToGroupRequest.Entity = new PlayFab.GroupsModels.EntityKey() { Id = "C28C53A93DE90602", Type = "title_player_account" };
        inviteToGroupRequest.AutoAcceptOutstandingApplication = true;
        inviteToGroupRequest.Group = new PlayFab.GroupsModels.EntityKey() { Id = "CBE02DC5CDA093FF", Type = "title_player_account" };

        PlayFabGroupsAPI.InviteToGroup(inviteToGroupRequest, 
            onSucces => 
            {
                print(onSucces.Group.Id);
            }, 
            onError => 
            {
                print(onError.ErrorMessage);
            });
    }

    private void Test4(PlayFab.GroupsModels.EntityKey entityKey)
    {
        AcceptGroupApplicationRequest acceptGroupApplicationRequest = new AcceptGroupApplicationRequest();
        acceptGroupApplicationRequest.Entity = entityKey;
        acceptGroupApplicationRequest.Group = new PlayFab.GroupsModels.EntityKey() { Id = "CBE02DC5CDA093FF", Type = "title_player_account" };

        PlayFabGroupsAPI.AcceptGroupApplication(acceptGroupApplicationRequest,
            s =>
            {

            },
            e =>
            {
                print(e.ErrorMessage);
            });

        //AddMembersRequest addMembersRequest = new AddMembersRequest();
        //addMembersRequest.Group = new PlayFab.GroupsModels.EntityKey();
        //addMembersRequest.Group = new PlayFab.GroupsModels.EntityKey() { Id = "CBE02DC5CDA093FF", Type = "title_player_account" };
        //addMembersRequest.Members = new List<PlayFab.GroupsModels.EntityKey>();
        //addMembersRequest.Members.Add(new PlayFab.GroupsModels.EntityKey { Id = entityKey .Id, Type = entityKey.Type});
        //PlayFabGroupsAPI.AddMembers(addMembersRequest, 
        //    s => 
        //    { 
                
        //    }, 
        //    e => 
        //    {
        //        print(e.ErrorMessage);
        //    });
    }

    public void Tesss()
    {
        
    }
}
