public abstract class BaseBoostZoneFeatures 
{
    public virtual void Use(BoostZoneManager boostZoneManager, TankController tankController)
    {
        OnTriggerEnter(tankController);
        boostZoneManager.Trigger(true);
        GlobalFunctions.DebugLog("true");
    }

    public virtual void Release(BoostZoneManager boostZoneManager, TankController tankController)
    {
        OnTriggerExit(tankController);
        boostZoneManager.Trigger(false);
        GlobalFunctions.DebugLog("false");
    }

    protected abstract void OnTriggerEnter(TankController tankController);
    protected abstract void OnTriggerExit(TankController tankController);
}
