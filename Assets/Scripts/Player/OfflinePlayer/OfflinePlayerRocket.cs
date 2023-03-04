using UnityEngine;

public class OfflinePlayerRocket : MonoBehaviour
{
    [SerializeField]
    protected BasePlayerTankController<BasePlayer> _playerTankController;



    protected virtual void OnEnable()
    {
        GlobalFunctions.Loop<DropBoxSelectionPanelRocket>.Foreach(GameSceneObjectsReferences.DropBoxSelectionPanelRockets, rocket => 
        {
            rocket.onRocket += OnRocket;
        });
    }

    protected virtual void OnDisable()
    {
        GlobalFunctions.Loop<DropBoxSelectionPanelRocket>.Foreach(GameSceneObjectsReferences.DropBoxSelectionPanelRockets, rocket =>
        {
            rocket.onRocket -= OnRocket;
        });
    }

    private void OnRocket(WeaponProperties weaponProperties, int id, int price)
    {
        _playerTankController._playerAmmoType.AddWeapon(weaponProperties);

        _playerTankController._scoreController.GetScore(price, null);
    }
}
