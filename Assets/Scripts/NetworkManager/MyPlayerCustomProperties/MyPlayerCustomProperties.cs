using Photon.Realtime;
using UnityEngine;

public partial class MyPlayerCustomProperties : MonoBehaviour
{   
    public static ExitGames.Client.Photon.Hashtable photonHashtable = new ExitGames.Client.Photon.Hashtable();
  

    public static void UpdatePlayerCustomProperties(Player localPlayer, object key, object value)
    {
        AddOrUpdatePhotonHashtable(key, value);
        localPlayer.CustomProperties = photonHashtable;
    }

    private static void AddOrUpdatePhotonHashtable(object key, object value)
    {
        if (photonHashtable.ContainsKey(key)) photonHashtable[key] = value;
        if (!photonHashtable.ContainsKey(key)) photonHashtable.Add(key, value);
    }
}
