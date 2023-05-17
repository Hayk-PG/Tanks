
public class OfflinePlayerXpUpgrader : PlayerDropBoxObserver
{
    private int _xp;




    protected override void Awake()
    {
        
    }

    protected override bool IsAllowed(DropBoxItemType dropBoxItemType)
    {
        return dropBoxItemType == DropBoxItemType.XpUpgrade;
    }

    protected override void Execute(object[] data)
    {
        RetrieveData(data);

        UpgradePlayerXp(_xp);
    }

    protected virtual void UpgradePlayerXp(int xp) => _playerTankController._scoreController.GetScore(xp, null, null, UnityEngine.Vector3.zero);
}
