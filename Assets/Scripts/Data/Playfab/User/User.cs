using System;
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

    public static void GetItems(string playfabId, Action<int[]> items)
    {
        new UserItems(playfabId, result => { items?.Invoke(result); });
    }

    public static void UpdateItems(string playfabId, int coins, int master, int strength)
    {
        new UserItems(playfabId, coins, master, strength);
    }
}
