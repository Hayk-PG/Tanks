using UnityEngine;
using PlayFab;
using PlayFab.ServerModels;
using System;
using System.Collections.Generic;

public partial class MyPlayfab : MonoBehaviour
{
    public Action<string, Action<string, Dictionary<string, string>>> OnUpdateReadOnlyData { get; set; }
    public Action<string, Action<string, Action<Dictionary<string, UserDataRecord>>>> OnGetReadOnlyData { get; set; }


    public void GetUserReadOnlyData(string playfabId, Action<Dictionary<string, UserDataRecord>> readOnlyData)
    {
        GetUserDataRequest gudr = new GetUserDataRequest();
        gudr.PlayFabId = playfabId;

        PlayFabServerAPI.GetUserReadOnlyData(gudr, 
            OnSuccess => 
            {
                readOnlyData?.Invoke(OnSuccess.Data);
            }, 
            onError => 
            {
                foreach (var error in onError.ErrorDetails)
                {
                    print(error);
                }
            });
    }

    public void UpdateUserDataRequest(string playfabId, Dictionary<string,string> newData)
    {
        UpdateUserDataRequest uudr = new UpdateUserDataRequest();
        uudr.PlayFabId = playfabId;
        uudr.Data = newData;

        PlayFabServerAPI.UpdateUserReadOnlyData(uudr, 
            onSucces => 
            {
                print("Success");
            }, 
            onError => 
            {
                foreach (var error in onError.ErrorDetails)
                {
                    print(error);
                }
            });
    }
}
