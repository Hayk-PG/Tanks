using UnityEngine;
using TMPro;

public class Toggle_RoomAccessibility : BaseSliderLevel<int>, IMatchmakeTextResult, IMatchmakeData
{
    [SerializeField] private TMP_InputField _passwordInputField;

    public override void OnSliderValueChanged()
    {
        UpdateTitleText(SliderValue);
        SetPasswordInputFieldInteractability(SliderValue);
    }

    protected override string Title(string prefix)
    {
        return prefix;
    }

    protected override string Result(int index)
    {
        return index == 0 ? GlobalFunctions.GreenColorText("public") : GlobalFunctions.RedColorText("private");
    }

    protected override void UpdateTitleText(int index)
    {
        _title.text = Title(Result(index));
    }

    private void SetPasswordInputFieldInteractability(int index)
    {
        _passwordInputField.interactable = index == 0 ? false : true;
    }

    public override void SetDefault()
    {
        SliderValue = 0;
        SetPasswordInputFieldInteractability(SliderValue);
        UpdateTitleText(SliderValue);
    }

    public string TextResultOnline()
    {
        return Keys.IsRoomPasswordSet + Result(SliderValue) + "\n";
    }

    public string TextResultOffline()
    {
        return "\n";
    }

    public void StoreData(MatchmakeData matchmakeData)
    {
        matchmakeData.IsRoomPublic = !_passwordInputField.interactable;
    }
}
