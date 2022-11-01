using UnityEngine;
using TMPro;

public class Toggle_RoomAccessibility : BaseSliderLevel<int>, IReset
{
    [SerializeField] private TMP_InputField _passwordInputField;

    public override void OnSliderValueChanged()
    {
        UpdateTitleText(Mathf.FloorToInt(_slider.value));
        SetPasswordInputFieldInteractability(Mathf.FloorToInt(_slider.value));
    }

    protected override void Activate()
    {
        
    }

    protected override string Title(string prefix)
    {
        return prefix;
    }

    protected override void UpdateTitleText(int index)
    {
        _title.text = Title(index == 0 ? "public" : "private");
    }

    private void SetPasswordInputFieldInteractability(int index)
    {
        _passwordInputField.interactable = index == 0 ? false : true;
    }

    public void SetDefault()
    {
        int defaultSliderValue = 0;

        _slider.value = defaultSliderValue;
        SetPasswordInputFieldInteractability(defaultSliderValue);
        UpdateTitleText(defaultSliderValue);
    }
}
