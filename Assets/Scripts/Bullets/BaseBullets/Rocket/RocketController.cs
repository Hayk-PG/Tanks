
public class RocketController : BaseBulletController
{
    public TankController Owner { get; protected set; }



    protected virtual void Start()
    {
        DetermineOwner();

        SwitchRocketControllerTab(true);
    }

    protected void DetermineOwner()
    {
        if (OwnerScore == default)
            return;

        Owner = GlobalFunctions.ObjectsOfType<TankController>.Find(tc => Get<IScore>.From(tc.gameObject) == OwnerScore);
    }

    public void SwitchRocketControllerTab(bool isActive)
    {
        if (Owner == null || Owner?.BasePlayer == null)
            return;

        GameSceneObjectsReferences.TabRocketController.SetActivity(isActive);
    }
}
