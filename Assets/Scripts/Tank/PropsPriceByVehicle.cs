using UnityEngine;

public class PropsPriceByVehicle : MonoBehaviour
{
    private TankController _tankController;
    private PropsTabCustomization _propsTabCustomization;
    private TileModifyManager _tileModifyManager;

    [SerializeField] [Range(0, 100)]
    private int _shieldPriceReducePercent, _tileModifyPriceReducePercent, _armoredCubePriceReducePercent, _armoredTilePriceReducePercent, _extendTilePriceReducePrecent;
    private int _oldRequiredScoreAmmount;

    public int ShieldPriceReducePercent
    {
        get => _shieldPriceReducePercent;
        set => _shieldPriceReducePercent = value;
    }
    public int TileModifyPriceReducePercent
    {
        get => _tileModifyPriceReducePercent;
        set => _tileModifyPriceReducePercent = value;
    }
    public int ArmoredCubePriceReducePercent
    {
        get => _armoredCubePriceReducePercent;
        set => _armoredCubePriceReducePercent = value;
    }
    public int ArmoredTilePriceReducePrecent
    {
        get => _armoredTilePriceReducePercent;
        set => _armoredTilePriceReducePercent = value;
    }

    public int ExtendTilePriceReducePercent
    {
        get => _extendTilePriceReducePrecent;
        set => _extendTilePriceReducePrecent = value;
    }
    


    private void Awake()
    {
        _tankController = Get<TankController>.From(gameObject);
        _propsTabCustomization = FindObjectOfType<PropsTabCustomization>();
        _tileModifyManager = FindObjectOfType<TileModifyManager>();
    }

    private void OnEnable()
    {
        _tankController.OnInitialize += OnInitialize;
    }

    private void OnDisable()
    {
        _tankController.OnInitialize -= OnInitialize;
    }

    private void OnInitialize()
    {
        UpdateRequirementsForPropsAndSupport();
        UpdateRequirementsForTileModifiers();
    }

    private int UpdatedRequirement(int oldRequiredScoreAmount, int percent)
    {
        int reducedAmount = oldRequiredScoreAmount / 100 * percent;
        return oldRequiredScoreAmount - reducedAmount;
    }

    private void UpdateRequirementsForPropsAndSupport()
    {
        if (_propsTabCustomization != null && _propsTabCustomization.InstantiatedTypeButtons.Length > 0)
        {
            GlobalFunctions.Loop<AmmoTypeButton>.Foreach(_propsTabCustomization.InstantiatedTypeButtons, typeButton => 
            {
                _oldRequiredScoreAmmount = typeButton._properties.RequiredScoreAmmount;

                if (typeButton._properties.SupportOrPropsType == Names.Shield)
                    typeButton._properties.RequiredScoreAmmount = UpdatedRequirement(_oldRequiredScoreAmmount, ShieldPriceReducePercent);
            });
        }
    }

    private void UpdateRequirementsForTileModifiers()
    {
        if(_tileModifyManager != null && _tileModifyManager.NewPrices.Length > 0)
        {
            GlobalFunctions.Loop<TileModifyManager.Prices>.Foreach(_tileModifyManager.NewPrices, props => 
            {
                _oldRequiredScoreAmmount = props.Price;

                if(props.Name == Names.ModifyGround)
                    props.Price = UpdatedRequirement(_oldRequiredScoreAmmount, TileModifyPriceReducePercent);

                if (props.Name == Names.MetalCube)
                    props.Price = UpdatedRequirement(_oldRequiredScoreAmmount, ArmoredCubePriceReducePercent);

                if (props.Name == Names.MetalGround)
                    props.Price = UpdatedRequirement(_oldRequiredScoreAmmount, ArmoredTilePriceReducePrecent);

                if (props.Name == Names.Bridge)
                    props.Price = UpdatedRequirement(_oldRequiredScoreAmmount, ExtendTilePriceReducePercent);
            });
        }
    }
}
