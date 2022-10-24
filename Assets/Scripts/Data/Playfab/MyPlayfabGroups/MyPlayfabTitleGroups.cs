using PlayFab;
using PlayFab.GroupsModels;
using System;

public class MyPlayfabTitleGroups
{
    public string DefualtMemberRoleID { get; private set; } = "members";
    public string DefaultMemberRoleName { get; private set; } = "Members";


    public void RequestToBecomeMember(TitleProperties titleGroupProperties, Action<bool> onResult)
    {
        IsMemberRequest isMemberRequest = new IsMemberRequest();
        isMemberRequest.Entity = new EntityKey() { Id = titleGroupProperties.MemberID, Type = titleGroupProperties.MemberType };
        isMemberRequest.Group = new EntityKey() { Id = titleGroupProperties.GroupID, Type = titleGroupProperties.GroupType };

        PlayFabGroupsAPI.IsMember(isMemberRequest,
            onSuccess =>
            {
                onResult?.Invoke(onSuccess.IsMember);
            },
            onError =>
            {
                GlobalFunctions.DebugLog(onError.ErrorMessage);
            });
    }

    public void Apply(TitleProperties titleGroupProperties, Action<bool> onFinalResult)
    {
        ApplyToGroupRequest applyToGroupRequest = new ApplyToGroupRequest();
        applyToGroupRequest.Entity = new EntityKey() { Id = titleGroupProperties.MemberID, Type = titleGroupProperties.MemberType };
        applyToGroupRequest.Group = new EntityKey() { Id = titleGroupProperties.GroupID, Type = titleGroupProperties.GroupType };

        PlayFabGroupsAPI.ApplyToGroup(applyToGroupRequest, 
            onSuccess => 
            {
            Accept(titleGroupProperties, onResult => { onFinalResult?.Invoke(onResult); });
            }, 
            onError => 
            {
                GlobalFunctions.DebugLog(onError.ErrorMessage);
            });
    }

    public void Accept(TitleProperties titleGroupProperties, Action<bool> onResult)
    {
        AcceptGroupApplicationRequest acceptGroupApplicationRequest = new AcceptGroupApplicationRequest();
        acceptGroupApplicationRequest.Entity = new EntityKey() { Id = titleGroupProperties.MemberID, Type = titleGroupProperties.MemberType };
        acceptGroupApplicationRequest.Group = new EntityKey() { Id = titleGroupProperties.GroupID, Type = titleGroupProperties.GroupType };

        PlayFabGroupsAPI.AcceptGroupApplication(acceptGroupApplicationRequest, 
            onSuccess => 
            {
                onResult?.Invoke(true);
            }, 
            onError =>
            {               
                onResult?.Invoke(false);
                GlobalFunctions.DebugLog(onError.ErrorMessage);
            });
    } 

    public void ListMembers(TitleProperties titleGroupProperties, Action<EntityMemberRole, EntityWithLineage> onResult)
    {
        ListGroupMembersRequest listGroupMembersRequest = new ListGroupMembersRequest();
        listGroupMembersRequest.Group = new EntityKey { Id = titleGroupProperties.GroupID, Type = titleGroupProperties.GroupType };

        PlayFabGroupsAPI.ListGroupMembers(listGroupMembersRequest, 
            onSuccess => 
            {
                GlobalFunctions.Loop<EntityMemberRole>.Foreach(onSuccess.Members, member => 
                {
                    GlobalFunctions.Loop<EntityWithLineage>.Foreach(member.Members, memberInfo =>
                    {
                        onResult?.Invoke(member, memberInfo);
                    });
                });
            }, 
            onError => 
            {
                onResult?.Invoke(null, null);
                GlobalFunctions.DebugLog(onError.ErrorMessage);
            });
    }

    public void Block(TitleProperties tileProperties)
    {
        BlockEntityRequest blockEntityRequest = new BlockEntityRequest();
        blockEntityRequest.Entity = new EntityKey { Id = tileProperties.MemberID, Type = tileProperties.MemberType };
        blockEntityRequest.Group = new EntityKey { Id = tileProperties.GroupID, Type = tileProperties.GroupType };

        PlayFabGroupsAPI.BlockEntity(blockEntityRequest, 
            onResult => 
            { 

            },
            onError => 
            { 

            });
    }
}
