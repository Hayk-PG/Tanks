using System;
using System.Collections.Generic;
using PlayFab;

public class UserStats 
{
    public UserStats(string playfabId, Action<Dictionary<string, int>> onResult)
    {
        PlayFab.ServerModels.GetPlayerStatisticsRequest getPlayerStatisticsRequest = new PlayFab.ServerModels.GetPlayerStatisticsRequest
        {
            PlayFabId = playfabId,
            StatisticNames = new List<string>
            {
                Keys.Level,
                Keys.Points,
                Keys.Wins,
                Keys.Losses,
                Keys.Kills,
                Keys.KD,
                Keys.TimePlayed,
                Keys.Quits
            }
        };

        Dictionary<string, int> statistics = new Dictionary<string, int>();

        foreach (var statsName in getPlayerStatisticsRequest.StatisticNames)
        {
            statistics.Add(statsName, 0);
        }

        PlayFabServerAPI.GetPlayerStatistics(getPlayerStatisticsRequest,
            onSuccess =>
            {
                for (int i = 0; i < onSuccess.Statistics.Count; i++)
                {
                    statistics[onSuccess.Statistics[i].StatisticName] = onSuccess.Statistics[i].Value;
                }

                onResult?.Invoke(statistics);
            },
            onError =>
            {
                GlobalFunctions.DebugLog("Failed to get stats");
                onResult?.Invoke(statistics);
            });
    }

    public UserStats(string playfabId, Dictionary<string, int> newStatistics, Action<bool> onResult)
    {
        new UserStats(playfabId, result =>
        {
            List<PlayFab.ServerModels.StatisticUpdate> statistics = new List<PlayFab.ServerModels.StatisticUpdate>();

            foreach (var item in result)
            {
                int value = item.Value;

                if (newStatistics != null && newStatistics.ContainsKey(item.Key))
                    value += newStatistics[item.Key];

                statistics.Add(new PlayFab.ServerModels.StatisticUpdate { StatisticName = item.Key, Value = item.Value });
            }

            PlayFab.ServerModels.UpdatePlayerStatisticsRequest updatePlayerStatisticsRequest = new PlayFab.ServerModels.UpdatePlayerStatisticsRequest
            {
                PlayFabId = playfabId,
                Statistics = statistics
            };

            PlayFabServerAPI.UpdatePlayerStatistics(updatePlayerStatisticsRequest,
                onSuccess =>
                {
                    onResult?.Invoke(true);
                },
                onError =>
                {
                    GlobalFunctions.DebugLog("Failed to update stats");
                    onResult?.Invoke(false);
                });
        });
    }
}
