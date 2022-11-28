using UnityEngine;

public enum SingleGameDifficultyLevel { Easy, Normal, Hard}
public enum GameWind { On, Off}
public enum RoundDuration { s10, s25, s30, s35, s45}

public partial class Data : MonoBehaviour
{
    public SingleGameDifficultyLevel SingleGameDifficultyLevel { get; private set; }
    public bool IsWindOn { get; private set; }
    public int RoundDuration { get; private set; }
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

    public void SaveRoundDuration(RoundDuration roundDuration)
    {
        RoundDuration = ConvertRoundDuration(roundDuration);
        SetData(new NewData { GameTime = RoundDuration });
    }

    public void SetMap(int index)
    {
        MapIndex = index;
    }

    public int ConvertRoundDuration(RoundDuration roundDuration)
    {
        return roundDuration == global::RoundDuration.s10 ? 10 : roundDuration == global::RoundDuration.s25 ? 25 : roundDuration == global::RoundDuration.s30 ? 30 : roundDuration == global::RoundDuration.s35 ? 35 : 45;
    }
}
