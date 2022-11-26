using UnityEngine;

public class GameDifficultyLevel : BaseSliderLevel<SingleGameDifficultyLevel>
{
    private void Start()
    {
        SetDefault();
    }

    protected override void ListenSliderValueChange()
    {
        Data.Manager.SetDifficultyLevel(SliderValue);
        UpdateTitleText(Data.Manager.SingleGameDifficultyLevel);
    }

    protected override void UpdateTitleText(SingleGameDifficultyLevel singleGameDifficultyLevel)
    {
        _title.text = Title(Result(singleGameDifficultyLevel));
    }

    protected override string Title(string suffix)
    {
        return suffix;
    }

    protected override string Result(SingleGameDifficultyLevel singleGameDifficultyLevel)
    {
        return singleGameDifficultyLevel == SingleGameDifficultyLevel.Easy ?
               "Easy" :
               singleGameDifficultyLevel == SingleGameDifficultyLevel.Normal ?
               "Normal" :
               "Hard";
    }

    public override void SetDefault()
    {
        SliderValue = PlayerPrefs.GetInt(Keys.DifficultyLevel, 0);
        Data.Manager.SetDifficultyLevel(SliderValue);
        UpdateTitleText(Data.Manager.SingleGameDifficultyLevel);
    }
}
