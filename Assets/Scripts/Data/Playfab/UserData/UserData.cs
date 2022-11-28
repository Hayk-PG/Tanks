using System;
using UnityEngine;

public class UserData : MonoBehaviour
{
    public static void GetItems(string playfabId, Action<int[]> items)
    {
        new UserItems(playfabId, result => { items?.Invoke(result); });
    }

    public static void UpdateItems(string playfabId, int coins, int master, int strength)
    {
        new UserItems(playfabId, coins, master, strength);
    }
}
