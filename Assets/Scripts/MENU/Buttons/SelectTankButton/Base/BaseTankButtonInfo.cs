using UnityEngine;

public abstract class BaseTankButtonInfo : MonoBehaviour
{
    protected virtual TanksInfo.Info Info(BaseSelectTankButton bstb)
    {
        return new TanksInfo.Info();
    }
    public abstract void TankNotOwnedScreen(BaseSelectTankButton bstb);
    public abstract void TankOwnedScreen(BaseSelectTankButton bstb);
}
