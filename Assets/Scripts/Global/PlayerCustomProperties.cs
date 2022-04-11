using Photon.Realtime;

public class PlayerCustomProperties 
{    
    public static void Update(Player player, object key, object value)
    {
        if (player.CustomProperties.ContainsKey(key))
            player.CustomProperties[key] = value;
        else
            player.CustomProperties.Add(key, value);
    }
}
