
public class SelectAiTankButton : BaseSelectTankButton
{
    protected override bool IsIndexCorrect()
    {
        return _index < _data.AvailableAITanks.Length;
    }

    public override void OnClickTankButton()
    {
        if (IsIndexCorrect())
            _data.SelectedAITankIndex = _data.AvailableAITanks[_index]._tankIndex;
    }
}
