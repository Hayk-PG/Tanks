
public class RocketController : BaseBulletController
{
    protected TankController _owner;



    protected virtual void Start()
    {
        DetermineOwner();

        SwitchRocketControllerTab(true);
    }

    protected void DetermineOwner()
    {
        if (OwnerScore == default)
            return;

        _owner = GlobalFunctions.ObjectsOfType<TankController>.Find(tc => Get<IScore>.From(tc.gameObject) == OwnerScore);
    }

    public void SwitchRocketControllerTab(bool isActive)
    {
        if (_owner == null || _owner?.BasePlayer == null)
            return;

        GameSceneObjectsReferences.TabRocketController.SetActivity(isActive);
    }
}
