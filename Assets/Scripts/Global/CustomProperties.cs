using Photon.Realtime;

public class CustomProperties 
{    
    public static void Player(Player player, object key, object value)
    {
        if (player.CustomProperties.ContainsKey(key))
            player.CustomProperties[key] = value;
        else
            player.CustomProperties.Add(key, value);
    }
}
