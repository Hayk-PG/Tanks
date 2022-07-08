using Photon.Realtime;

public class CustomProperties 
{    
    public static void Add(Player player, object key, object value)
    {
        if (player.CustomProperties.ContainsKey(key))
            player.CustomProperties[key] = value;
        else
            player.CustomProperties.Add(key, value);
    }

    public static void Delete(Player player, object key)
    {
        if (player.CustomProperties.ContainsKey(key))
            player.CustomProperties.Remove(key);
    }
}
