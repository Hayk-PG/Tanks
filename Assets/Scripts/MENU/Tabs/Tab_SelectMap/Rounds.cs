using UnityEngine;

public class Rounds : BaseSliderLevel<int>
{
    protected override void Awake()
    {
        UpdateTitleText(0);
    }

    public override void OnSliderValueChanged()
    {
        UpdateTitleText(Mathf.FloorToInt(_slider.value));
    }

    protected override void Activate()
    {
        
    }

    protected override string Title(string suffix)
    {
        return "Rounds " + "[" + suffix + "]";
    }

    protected override void UpdateTitleText(int index)
    {
        int rounds = index == 0 ? 1 : index == 1 ? 3 : 5;
        _title.text = Title("<color=#C25700>" + rounds + "</color>");
    }
}
