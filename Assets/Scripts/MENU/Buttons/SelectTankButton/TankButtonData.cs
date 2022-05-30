
public class TankButtonData : BaseTankButtonData
{
    public override void Save(BaseSelectTankButton bstb)
    {
        bstb._data.SetData(new Data.NewData { SelectedTankIndex = bstb._data.AvailableTanks[bstb._index]._tankIndex });
    }
}
