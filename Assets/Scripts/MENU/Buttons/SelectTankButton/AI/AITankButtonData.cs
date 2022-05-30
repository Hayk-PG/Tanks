
public class AITankButtonData : BaseTankButtonData
{
    public override void Save(BaseSelectTankButton bstb)
    {
        bstb._data.SelectedAITankIndex = bstb._data.AvailableAITanks[bstb._index]._tankIndex;
    }
}
