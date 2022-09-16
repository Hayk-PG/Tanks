using UnityEngine;

public class PropsPriceByVehicle : MonoBehaviour
{
    private TankController _tankController;
    private PropsTabCustomization _propsTabCustomization;
    private Tab_TileModify _tabTileModify;

    [SerializeField] [Range(0, 100)]
    private int _shieldPriceReducePercent, _tileModifyPriceReducePercent, _armoredCubePriceReducePercent, _armoredTilePriceReducePercent;
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

    


    private void Awake()
    {
        _tankController = Get<TankController>.From(gameObject);
        _propsTabCustomization = FindObjectOfType<PropsTabCustomization>();
        _tabTileModify = FindObjectOfType<Tab_TileModify>();
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
        DefineNewRequiredScoreAmounts();
        DefineNewTileModifyScoreAmounts();
    }

    private int NewRequiredScoreAmount(int oldRequiredScoreAmount, int percent)
    {
        int reducedAmount = oldRequiredScoreAmount / 100 * percent;
        return oldRequiredScoreAmount - reducedAmount;
    }

    private void DefineNewRequiredScoreAmounts()
    {
        if (_propsTabCustomization != null && _propsTabCustomization.InstantiatedTypeButtons.Length > 0)
        {
            GlobalFunctions.Loop<AmmoTypeButton>.Foreach(_propsTabCustomization.InstantiatedTypeButtons, typeButton => 
            {
                _oldRequiredScoreAmmount = typeButton._properties.RequiredScoreAmmount;

                if (typeButton._properties.SupportOrPropsType == Names.Shield)
                    typeButton._properties.RequiredScoreAmmount = NewRequiredScoreAmount(_oldRequiredScoreAmmount, ShieldPriceReducePercent);
            });
        }
    }

    private void DefineNewTileModifyScoreAmounts()
    {
        if(_tabTileModify != null && _tabTileModify.NewPrices.Length > 0)
        {
            GlobalFunctions.Loop<Tab_TileModify.Prices>.Foreach(_tabTileModify.NewPrices, props => 
            {
                _oldRequiredScoreAmmount = props.Price;

                if(props.Name == Names.ModifyGround)
                    props.Price = NewRequiredScoreAmount(_oldRequiredScoreAmmount, TileModifyPriceReducePercent);

                if (props.Name == Names.MetalCube)
                    props.Price = NewRequiredScoreAmount(_oldRequiredScoreAmmount, ArmoredCubePriceReducePercent);

                if (props.Name == Names.MetalGround)
                    props.Price = NewRequiredScoreAmount(_oldRequiredScoreAmmount, ArmoredTilePriceReducePrecent);
            });
        }
    }
}
