using UnityEngine;

public abstract class BaseTankButtonInfo : MonoBehaviour
{
    protected virtual TanksInfo.Info Info(BaseSelectTankButton bstb)
    {
        return new TanksInfo.Info();
    }
    protected virtual TanksInfo.RequiredItemsInfo RequiredItemsInfo(BaseSelectTankButton bstb)
    {
        return new TanksInfo.RequiredItemsInfo();
    }
    public abstract void TankNotOwnedScreen(BaseSelectTankButton bstb);
    public abstract void TankOwnedScreen(BaseSelectTankButton bstb);
    public abstract void RequiredItemsScreen(BaseSelectTankButton bstb);
}
