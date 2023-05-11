using System;
using System.Collections.Generic;

public interface IGetPointsAndAmmoDataFromPlayer
{
    Action<Action<List<int>>> OnGetPointsAndAmmoDataFromPlayer { get; set; }

    void GetPointsAndAmmoDataFromPlayer(List<int> bulletsCount);
}
