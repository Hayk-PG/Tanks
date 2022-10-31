using UnityEngine;

public class GameDifficultyLevel : BaseSliderLevel<SingleGameDifficultyLevel>
{
    protected override void Activate()
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, MyPhotonNetwork.IsOfflineMode);
        _slider.value = PlayerPrefs.GetInt(Keys.DifficultyLevel, 0);
        Data.Manager.SetDifficultyLevel(Mathf.FloorToInt(_slider.value));
        UpdateTitleText(Data.Manager.SingleGameDifficultyLevel);
    }

    protected override string Title(string suffix)
    {
        return "Difficulty Level " + "[" + suffix + "]";
    }

    protected override void UpdateTitleText(SingleGameDifficultyLevel singleGameDifficultyLevel)
    {
        switch (singleGameDifficultyLevel)
        {
            case SingleGameDifficultyLevel.Easy: _title.text = Title("<color=#80BBF7>" + "Easy" + "</color>"); break;
            case SingleGameDifficultyLevel.Normal: _title.text = Title("<color=#80BBF7>" + "Normal" + "</color>"); break;
            case SingleGameDifficultyLevel.Hard: _title.text = Title("<color=#80BBF7>" + "Hard" + "</color>"); break;
        }
    }

    public override void OnSliderValueChanged()
    {
        Data.Manager.SetDifficultyLevel(Mathf.FloorToInt(_slider.value));
        UpdateTitleText(Data.Manager.SingleGameDifficultyLevel);
    }
}
