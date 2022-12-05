using System;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour
{
    public static void Register(string userId, string userPassword, string userEmail, Action<PlayFab.ClientModels.RegisterPlayFabUserResult> onResult)
    {
        new UserRegister(userId, userPassword, userEmail, result => { onResult?.Invoke(result); });
    }

    public static void Login(string userId, string userPassword, Action<PlayFab.ClientModels.LoginResult> onResult)
    {
        new UserLogin(userId, userPassword, result => { onResult?.Invoke(result); });
    }

    public static void GetStats(string playfabId, Action<Dictionary<string, int>> onResult)
    {
        new UserStats(playfabId, result => { onResult?.Invoke(result); });
    }

    public static void UpdateStats(string playfabId, Dictionary<string, int> newStatistics, Action<Dictionary<string, int>> onUpdatedStatisticsOutput)
    {
        new UserStats(playfabId, newStatistics, result => { onUpdatedStatisticsOutput?.Invoke(result); });
    }

    public static void GetItems(string playfabId, Action<int[]> items)
    {
        new UserItems(playfabId, result => { items?.Invoke(result); });
    }

    public static void UpdateItems(string playfabId, int coins, int master, int strength)
    {
        new UserItems(playfabId, coins, master, strength);
    }
}
