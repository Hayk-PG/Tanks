using System;
using System.Collections.Generic;

public class UserItems 
{
    public UserItems(string playfabId, Action<int[]> onResult)
    {
        int coinsInit = 0;
        int masterInit = 0;
        int strengthInit = 0;

        new UserData(playfabId, result =>
        {
            if (result != null && result.ContainsKey(Keys.ItemCoins) && result.ContainsKey(Keys.ItemMaster) && result.ContainsKey(Keys.ItemStrength))
            {
                if (int.TryParse(result[Keys.ItemCoins].Value, out int itemsCoins))
                    coinsInit = itemsCoins;

                if (int.TryParse(result[Keys.ItemMaster].Value, out int itemsMaster))
                    masterInit = itemsMaster;

                if (int.TryParse(result[Keys.ItemStrength].Value, out int itemsStrength))
                    strengthInit = itemsStrength;
            }

            onResult?.Invoke(new int[] { coinsInit, masterInit, strengthInit });
        });
    }

    public UserItems(string playfabId, int coins, int master, int strength)
    {
        new UserItems(playfabId, result =>
        {
            Dictionary<string, string> items = new Dictionary<string, string>
            {
                { Keys.ItemCoins,  (result[0] + coins).ToString() },
                { Keys.ItemMaster, (result[1] + master).ToString() },
                { Keys.ItemStrength, (result[2] + strength).ToString() }
            };

            new UserData(playfabId, PlayFab.ServerModels.UserDataPermission.Public, items, null);
        });
    }
}
