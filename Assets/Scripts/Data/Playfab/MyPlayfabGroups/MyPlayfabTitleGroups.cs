using PlayFab;
using PlayFab.GroupsModels;
using System;

public class MyPlayfabTitleGroups
{
    public void RequestToBecomeMember(TitleGroupProperties titleGroupProperties, Action<bool> onResult)
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

    public void Apply(TitleGroupProperties titleGroupProperties, Action<bool> onFinalResult)
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

    public void Accept(TitleGroupProperties titleGroupProperties, Action<bool> onResult)
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

    public void ListMembers(TitleGroupProperties titleGroupProperties, Action<ListGroupMembersResponse> onResult)
    {
        ListGroupMembersRequest listGroupMembersRequest = new ListGroupMembersRequest();
        listGroupMembersRequest.Group = new EntityKey { Id = titleGroupProperties.GroupID, Type = titleGroupProperties.GroupType };

        PlayFabGroupsAPI.ListGroupMembers(listGroupMembersRequest, 
            onSuccess => 
            {
                onResult?.Invoke(onSuccess);
            }, 
            onError => 
            {
                onResult?.Invoke(null);
                GlobalFunctions.DebugLog(onError.ErrorMessage);
            });
    }
}
