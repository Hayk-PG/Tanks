using System;
using System.Collections.Generic;

public interface IGetPointsAndAmmoDataFromPlayer
{
    Action<Action<int, List<int>>> OnGetPointsAndAmmoDataFromPlayer { get; set; }

    void GetPointsAndAmmoDataFromPlayer(int playerPoints, List<int> bulletsCount);
}
