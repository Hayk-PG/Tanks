using UnityEngine;
using PlayFab;
using PlayFab.ProfilesModels;
using System;

public class MyPlayfabEntity
{
    public void GetPlayerProfileFromEntity(string entityId, string entityType, Action<GetEntityProfileResponse> onResult)
    {
        GetEntityProfileRequest getEntityProfileRequest = new GetEntityProfileRequest();
        getEntityProfileRequest.Entity = new EntityKey() { Id = entityId, Type = entityType };

        PlayFabProfilesAPI.GetProfile(getEntityProfileRequest,
            onSuccess =>
            {
                onResult?.Invoke(onSuccess);
            },
            onError =>
            {
                Debug.Log(onError.ErrorMessage);
            });
    }
}
