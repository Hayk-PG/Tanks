using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName = "Scriptable objects/Tanks/New tank")]

//ADDRESSABLE
public class TankProperties : ScriptableObject
{
    public int _tankIndex;

    [Header("Addressable")]
    public AssetReferenceSprite assetReferenceIcon;

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
    public float _fuelConsumptionPercentage;

    [Space]
    public float _speedOnNormal;
    public float _speedOnRain;
    public float _speedOnSnow;
    public float _brakeOnNormal;
    public float _brakeOnRain;
    public float _brakeOnSnow;
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

    [Header("Tile modifiers percent cost")]
    public int _requiredTileModifierCostPercentage;
    public int _requiredArmoredCubeCostPercentage;
    public int _requiredArmoredTileCostPercentage;
    public int _requiredTileExtenderCostPercentage;

    [Header("Tank btn stats")]
    public float _mobility;
    public float _protection;
    public float _firePower;


    public virtual void GetValuesFromTankPrefab()
    {
        TankInfo tankInfo = Get<TankInfo>.From(_tank.gameObject);
        Rigidbody rigidbody = Get<Rigidbody>.From(_tank.gameObject);
        BaseTankMovement baseTankMovement = Get<BaseTankMovement>.From(_tank.gameObject);
        BaseShootController baseShootController = Get<BaseShootController>.From(_tank.gameObject);
        PlayerAmmoType playerAmmoType = Get<PlayerAmmoType>.From(_tank.gameObject);
        HealthController healthController = Get<HealthController>.From(_tank.gameObject);
        TileModifierPercentCost tileModifierPercentCost = Get<TileModifierPercentCost>.From(_tank.gameObject);

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

            _speedOnNormal = baseTankMovement._speedOnNormal;
            _speedOnRain = baseTankMovement._speedOnRain;
            _speedOnSnow = baseTankMovement._speedOnSnow;
            _brakeOnNormal = baseTankMovement._brakeOnNormal;
            _brakeOnRain = baseTankMovement._brakeOnRain;
            _brakeOnSnow = baseTankMovement._brakeOnSnow;

            _fuelConsumptionPercentage = baseTankMovement.fuelConsumptionPercent;
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

        if(tileModifierPercentCost != null)
        {
            _requiredTileModifierCostPercentage = tileModifierPercentCost.RequiredTileModifierCostPercentage;
            _requiredArmoredCubeCostPercentage = tileModifierPercentCost.RequiredArmoredCubeCostPercentage;
            _requiredArmoredTileCostPercentage = tileModifierPercentCost.RequiredArmoredTileCostPercentage;
            _requiredTileExtenderCostPercentage = tileModifierPercentCost.RequiredTileExtenderCostPercentage;
        }

        CalculateTankBtnStats();


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
            TileModifierPercentCost tileModifierPercentCost = Get<TileModifierPercentCost>.From(_aiTank.gameObject);

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

                baseTankMovement._speedOnNormal = _speedOnNormal;
                baseTankMovement._speedOnRain = _speedOnRain;
                baseTankMovement._speedOnSnow = _speedOnSnow;
                baseTankMovement._brakeOnNormal = _brakeOnNormal;
                baseTankMovement._brakeOnRain = _brakeOnRain;
                baseTankMovement._brakeOnSnow = _brakeOnSnow;

                baseTankMovement.fuelConsumptionPercent = _fuelConsumptionPercentage;
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

            if (tileModifierPercentCost != null)
            {
                tileModifierPercentCost.RequiredTileModifierCostPercentage = _requiredTileModifierCostPercentage;
                tileModifierPercentCost.RequiredArmoredCubeCostPercentage = _requiredArmoredCubeCostPercentage;
                tileModifierPercentCost.RequiredArmoredTileCostPercentage = _requiredArmoredTileCostPercentage;
                tileModifierPercentCost.RequiredTileExtenderCostPercentage = _requiredTileExtenderCostPercentage;
            }
        }
    }

    private void CalculateTankBtnStats()
    {
        CalculateMoblity();

        CalculateProtection();

        CalculateFirePower();
    }

    private void CalculateMoblity()
    {
        float normalSpeedLimit = 1000;
        float accelerateLimit = 3000;
        float damageFactorLimit = 100;

        // If the _fuelConsumptionPercentage is less than or equal to 100, it means that no additional number will be added to the fuelConsumptionPercentageLimit, and therefore it will be set to 0. 
        // If the _fuelConsumptionPercentage is greater than 100, the additional amount will be calculated using the formula '_fuelConsumptionPercentage - 100' and added to the fuelConsumptionPercentageLimit.

        float fuelConsumptionPercentageLimit = _fuelConsumptionPercentage <= 100 ? 0 : _fuelConsumptionPercentage - 100;

        _mobility = Mathf.InverseLerp(0, normalSpeedLimit + accelerateLimit + damageFactorLimit + fuelConsumptionPercentageLimit, _normalSpeed + _accelerated + _damageFactor);
    }

    private void CalculateProtection()
    {
        float protectionLimit = 50;

        _protection = Mathf.InverseLerp(0, protectionLimit, _armor);
    }

    private void CalculateFirePower()
    {        
        float powerLimit = 0f;
        float currenPower = 0f;

        foreach (var item in _weapons)
        {
            currenPower += item._damageValue + item._radius + item._destructDamage + item._bulletMaxForce;
            powerLimit += 100f + 1f + 100f + 20f;
        }

        _firePower = Mathf.InverseLerp(0, powerLimit, currenPower);
    }
}
