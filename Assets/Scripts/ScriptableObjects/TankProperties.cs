using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable objects/Tanks/New tank")]
public class TankProperties : ScriptableObject
{
    public int _tankIndex;

    [Header("UI")]
    public Sprite _iconTank;

    [Header("Prefab")]
    public TankController _tank;

    [Header("Same type AI Tank prefab")]
    public AITankController _aiTank;

    [Header("Tank info")]
    public string _tankName;
    public int _starsCount;
    public int _getItNowPrice;
    public int _initialBuildHours;
    public int _initialBuildMinutes;
    public int _initialBuildSeconds;
    public TankInfo.Items[] _requiredItems;
    public int _availableInLevel;

    [Header("Tank prefab movement parameters")]
    public float _rigidbodyMass;
    public float _normalSpeed;
    public float _maxBrake;
    public float _accelerated;
    public int _damageFactor;
    public Vector3 _normalCenterOfMass;

    [Header("Tank prefab canon parameters")]
    public float _minEulerAngleX;
    public float _maxEulerAngleX;
    public float _rotationSpeed;
    public Vector3 _rotationStabilizer;

    [Header("Tank prefab shoot parameters")]
    public float _rigidbodyForceMultiplier;

    [Header("Weapons")]
    public WeaponProperties[] _weapons;

    [Header("Health")]
    public int _armor;

    [Header("Props cut")]
    public int _shieldCutPerecent;
    public int _tileModifyCutPercent;
    public int _armoredCubeCutPercent;
    public int _armoredTileCutPercent;
    public int _extendTileCutPercent;


    public virtual void GetValuesFromTankPrefab()
    {
        TankInfo tankInfo = Get<TankInfo>.From(_tank.gameObject);
        Rigidbody rigidbody = Get<Rigidbody>.From(_tank.gameObject);
        BaseTankMovement baseTankMovement = Get<BaseTankMovement>.From(_tank.gameObject);
        BaseShootController baseShootController = Get<BaseShootController>.From(_tank.gameObject);
        PlayerAmmoType playerAmmoType = Get<PlayerAmmoType>.From(_tank.gameObject);
        HealthController healthController = Get<HealthController>.From(_tank.gameObject);
        PropsPriceByVehicle propsPriceByVehicle = Get<PropsPriceByVehicle>.From(_tank.gameObject);

        if (tankInfo != null)
        {
            _tankName = tankInfo.TankName;
            _starsCount = tankInfo.StarsCount;
            _getItNowPrice = tankInfo.GetItNowPrice;
            _initialBuildHours = tankInfo.InitialBuildHours;
            _initialBuildMinutes = tankInfo.InitialBuildMinutes;
            _initialBuildSeconds = tankInfo.InitialBuildSeconds;
            _requiredItems = tankInfo.RequiredItems;
            _availableInLevel = tankInfo.AvailableInLevel;
        }

        if(rigidbody != null)
        {
            _rigidbodyMass = rigidbody.mass;
        }

        if(baseTankMovement != null)
        {
            _normalSpeed = baseTankMovement._normalSpeed;
            _maxBrake = baseTankMovement._maxBrake;
            _accelerated = baseTankMovement._accelerated;
            _damageFactor = baseTankMovement._damageFactor;
            _normalCenterOfMass = baseTankMovement._normalCenterOfMass;
        }

        if(baseShootController != null)
        {
            _minEulerAngleX = baseShootController._canon._minEulerAngleX;
            _maxEulerAngleX = baseShootController._canon._maxEulerAngleX;
            _rotationSpeed = baseShootController._canon._rotationSpeed;
            _rotationStabilizer = baseShootController._canon._rotationStabilizer;
            _rigidbodyForceMultiplier = baseShootController._shoot._rigidbodyForceMultiplier;
        }

        if(playerAmmoType != null)
        {
            _weapons = playerAmmoType._weapons;
        }

        if (healthController != null)
        {
            _armor = healthController.Armor;
        }

        if(propsPriceByVehicle != null)
        {
            _shieldCutPerecent = propsPriceByVehicle.ShieldPriceReducePercent;
            _tileModifyCutPercent = propsPriceByVehicle.TileModifyPriceReducePercent;
            _armoredCubeCutPercent = propsPriceByVehicle.ArmoredCubePriceReducePercent;
            _armoredTileCutPercent = propsPriceByVehicle.ArmoredTilePriceReducePrecent;
            _extendTileCutPercent = propsPriceByVehicle.ExtendTilePriceReducePercent;
        }

#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }

    public virtual void ApplyValuesToSameTypeAITank()
    {
        if (_aiTank != null)
        {
            TankInfo tankInfo = Get<TankInfo>.From(_aiTank.gameObject);
            Rigidbody rigidbody = Get<Rigidbody>.From(_aiTank.gameObject);
            BaseTankMovement baseTankMovement = Get<BaseTankMovement>.From(_aiTank.gameObject);
            BaseShootController baseShootController = Get<BaseShootController>.From(_aiTank.gameObject);
            HealthController healthController = Get<HealthController>.From(_aiTank.gameObject);          

            if (tankInfo != null)
            {
                tankInfo.TankName = _tankName;
                tankInfo.StarsCount = _starsCount;
                tankInfo.GetItNowPrice = _getItNowPrice;
                tankInfo.InitialBuildHours = _initialBuildHours;
                tankInfo.InitialBuildMinutes = _initialBuildMinutes;
                tankInfo.InitialBuildSeconds = _initialBuildSeconds;
                tankInfo.RequiredItems = _requiredItems;
                tankInfo.AvailableInLevel = _availableInLevel;
            }

            if (rigidbody != null)
                rigidbody.mass = _rigidbodyMass;

            if (baseTankMovement != null)
            {
                baseTankMovement._normalSpeed = _normalSpeed;
                baseTankMovement._accelerated = _accelerated;
                baseTankMovement._maxBrake = _maxBrake;
                baseTankMovement._normalCenterOfMass = _normalCenterOfMass;
            }

            if (baseShootController != null)
            {
                baseShootController._canon._minEulerAngleX = _minEulerAngleX;
                baseShootController._canon._maxEulerAngleX = _maxEulerAngleX;
                baseShootController._shoot._rigidbodyForceMultiplier = _rigidbodyForceMultiplier;
            }

            if (healthController != null)
            {
                healthController.Armor = _armor;
            }
        }
    }
}
