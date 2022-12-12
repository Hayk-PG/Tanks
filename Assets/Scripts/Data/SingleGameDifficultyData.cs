using UnityEngine;

public enum SingleGameDifficultyLevel { Easy, Normal, Hard}
public enum GameWind { On, Off}
public enum RoundTimeStates { s10, s25, s30, s35, s45}

public partial class Data : MonoBehaviour
{
    public SingleGameDifficultyLevel SingleGameDifficultyLevel { get; private set; }
    public bool IsWindOn { get; private set; }
    public int RoundTime { get; private set; }
    public int MapIndex { get; private set; }


    public void SaveGameDifficultyLevel(int difficultyLevel)
    {
        SingleGameDifficultyLevel = (SingleGameDifficultyLevel)difficultyLevel;
        SetData(new NewData { DifficultyLevel = difficultyLevel });
    }

    public void SaveWind(GameWind gameWind)
    {
        IsWindOn = gameWind == GameWind.On ? true : false;
        SetData(new NewData { IsWindToggleOn = (int)gameWind });
    }

    public void SaveRoundTime(RoundTimeStates roundTime)
    {
        RoundTime = ConvertRoundTimeStates(roundTime);
        SetData(new NewData { RoundTime = RoundTime });
    }

    public void SetMap(int index)
    {
        MapIndex = index;
    }

    public int ConvertRoundTimeStates(RoundTimeStates roundTime)
    {
        return roundTime == global::RoundTimeStates.s10 ? 10 : roundTime == global::RoundTimeStates.s25 ? 25 : roundTime == global::RoundTimeStates.s30 ? 30 : roundTime == global::RoundTimeStates.s35 ? 35 : 45;
    }

    public RoundTimeStates ConvertRoundTimeStates(int roundDuration)
    {
        return roundDuration == 10 ? global::RoundTimeStates.s10 : roundDuration == 25 ? global::RoundTimeStates.s25 : roundDuration == 30 ? global::RoundTimeStates.s30 :
               roundDuration == 35 ? global::RoundTimeStates.s35 : global::RoundTimeStates.s45;
    }
}
