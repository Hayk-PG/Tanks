
public class OfflinePlayerXpUpgrader : PlayerDropBoxObserver
{
    private int _xp;

    protected override bool IsAllowed(DropBoxItemType dropBoxItemType)
    {
        return dropBoxItemType == DropBoxItemType.XpUpgrade;
    }

    protected override void Execute(object[] data)
    {
        _xp = (int)data[0];
        _price = (int)data[1];

        UpgradePlayerXp(_xp);
    }

    protected virtual void UpgradePlayerXp(int xp) => _playerTankController._scoreController.GetScore(xp, null, null, UnityEngine.Vector3.zero);
}
