
public class TankButtonInfo : BaseTankButtonInfo
{
    protected override TanksInfo.Info Info(BaseSelectTankButton bstb)
    {
        return new TanksInfo.Info(
                (int)bstb._data.AvailableTanks[bstb._index]._normalSpeed,
                (int)bstb._data.AvailableTanks[bstb._index]._maxForce,
                bstb._data.AvailableTanks[bstb._index]._armor,

                bstb._data.AvailableTanks[bstb._index]._getItNowPrice,
                Converter.HhMMSS(bstb._data.AvailableTanks[bstb._index]._initialBuildHours, bstb._data.AvailableTanks[bstb._index]._initialBuildMinutes, bstb._data.AvailableTanks[bstb._index]._initialBuildSeconds));
    }

    public override void TankNotOwnedScreen(BaseSelectTankButton bstb)
    {
        bstb._tanksInfo.TankNotOwnedScreen(Info(bstb));
    }

    public override void TankOwnedScreen(BaseSelectTankButton bstb)
    {
        bstb._tanksInfo.TankOwnedScreen(Info(bstb));
    }
}
