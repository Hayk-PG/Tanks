using System;

public interface IGetPlayerPoints
{
    Action<Action<int>> OnGetPlayerPointsCount { get; set; }

    void GetPlayerPoints(int playerPoints);
}
