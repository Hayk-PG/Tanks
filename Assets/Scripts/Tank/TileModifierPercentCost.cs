using UnityEngine;

public class TileModifierPercentCost : MonoBehaviour
{
    private TankController _tankController;

    [SerializeField] [Range(50, 500)] [Space]
    private int _requiredTileModifierCostPercentage, _requiredArmoredCubeCostPercentage, _requiredArmoredTileCostPercentage, _requiredTileExtenderCostPercentage;

    public int RequiredTileModifierCostPercentage
    {
        get => _requiredTileModifierCostPercentage;
        set => _requiredTileModifierCostPercentage = value;
    }

    public int RequiredArmoredCubeCostPercentage
    {
        get => _requiredArmoredCubeCostPercentage;
        set => _requiredArmoredCubeCostPercentage = value;
    }

    public int RequiredArmoredTileCostPercentage
    {
        get => _requiredArmoredTileCostPercentage;
        set => _requiredArmoredTileCostPercentage = value;
    }

    public int RequiredTileExtenderCostPercentage
    {
        get => _requiredTileExtenderCostPercentage;
        set => _requiredTileExtenderCostPercentage = value;
    }

    




    private void Awake() => GetTankController();

    private void Start() => SetRequiredCostPercentageForTileModifications();

    private void GetTankController() => _tankController = Get<TankController>.From(gameObject);

    private void SetRequiredCostPercentageForTileModifications()
    {
        bool isAiTank = MyPhotonNetwork.IsOfflineMode && gameObject.name == Names.Tank_SecondPlayer;
        bool isNonPlayerTank = _tankController?.BasePlayer == null;

        if (isAiTank || isNonPlayerTank)
            return;

        GameSceneObjectsReferences.TileModifyManager.SetRquiredCost(_requiredTileModifierCostPercentage, _requiredArmoredCubeCostPercentage, _requiredArmoredTileCostPercentage, _requiredTileExtenderCostPercentage);
    }
}
