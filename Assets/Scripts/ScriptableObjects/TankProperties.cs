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

    [Header("Health")]
    public int _armor;

    [Header("Tank info")]
    public string _tankName;
    public int _starsCount;
    public int _getItNowPrice;
    public int _initialBuildHours;
    public int _initialBuildMinutes;
    public int _initialBuildSeconds;
    public TankInfo.Items[] _requiredItems;
    public int _availableInLevel;


    public virtual void GetValuesFromTankPrefab()
    {
        _rigidbodyMass = Get<Rigidbody>.From(_tank.gameObject).mass; 
        _normalSpeed = Get<BaseTankMovement>.From(_tank.gameObject)._normalSpeed;
        _maxBrake = Get<BaseTankMovement>.From(_tank.gameObject)._maxBrake;
        _accelerated = Get<BaseTankMovement>.From(_tank.gameObject)._accelerated;
        _damageFactor = Get<BaseTankMovement>.From(_tank.gameObject)._damageFactor;
        _normalCenterOfMass = Get<BaseTankMovement>.From(_tank.gameObject)._normalCenterOfMass;

        _minEulerAngleX = Get<BaseShootController>.From(_tank.gameObject)._canon._minEulerAngleX;
        _maxEulerAngleX = Get<BaseShootController>.From(_tank.gameObject)._canon._maxEulerAngleX;
        _rotationSpeed = Get<BaseShootController>.From(_tank.gameObject)._canon._rotationSpeed;
        _rotationStabilizer = Get<BaseShootController>.From(_tank.gameObject)._canon._rotationStabilizer;

        _armor = Get<HealthController>.From(_tank.gameObject).Armor;

        _tankName = Get<TankInfo>.From(_tank.gameObject).TankName;
        _starsCount = Get<TankInfo>.From(_tank.gameObject).StarsCount;
        _getItNowPrice = Get<TankInfo>.From(_tank.gameObject).GetItNowPrice;
        _initialBuildHours = Get<TankInfo>.From(_tank.gameObject).InitialBuildHours;
        _initialBuildMinutes = Get<TankInfo>.From(_tank.gameObject).InitialBuildMinutes;
        _initialBuildSeconds = Get<TankInfo>.From(_tank.gameObject).InitialBuildSeconds;
        _requiredItems = Get<TankInfo>.From(_tank.gameObject).RequiredItems;
        _availableInLevel = Get<TankInfo>.From(_tank.gameObject).AvailableInLevel;

#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }

    public virtual void ApplyValuesToSameTypeAITank()
    {
        if (_aiTank != null)
        {
            if (Get<Rigidbody>.From(_aiTank.gameObject) != null)
                Get<Rigidbody>.From(_aiTank.gameObject).mass = _rigidbodyMass;

            if (Get<BaseTankMovement>.From(_aiTank.gameObject) != null)
            {
                Get<BaseTankMovement>.From(_aiTank.gameObject)._normalSpeed = _normalSpeed;
                Get<BaseTankMovement>.From(_aiTank.gameObject)._accelerated = _accelerated;
                Get<BaseTankMovement>.From(_aiTank.gameObject)._maxBrake = _maxBrake;
                Get<BaseTankMovement>.From(_aiTank.gameObject)._normalCenterOfMass = _normalCenterOfMass;
            }

            if (Get<BaseShootController>.From(_aiTank.gameObject) != null)
            {
                Get<BaseShootController>.From(_aiTank.gameObject)._canon._minEulerAngleX = _minEulerAngleX;
                Get<BaseShootController>.From(_aiTank.gameObject)._canon._maxEulerAngleX = _maxEulerAngleX;
            }

            if (Get<HealthController>.From(_aiTank.gameObject) != null)
            {
                Get<HealthController>.From(_aiTank.gameObject).Armor = _armor;
            }

            if (Get<TankController>.From(_aiTank.gameObject) != null)
            {
                Get<TankInfo>.From(_aiTank.gameObject).TankName = _tankName;
                Get<TankInfo>.From(_aiTank.gameObject).StarsCount = _starsCount;
                Get<TankInfo>.From(_aiTank.gameObject).GetItNowPrice = _getItNowPrice;
                Get<TankInfo>.From(_aiTank.gameObject).InitialBuildHours = _initialBuildHours;
                Get<TankInfo>.From(_aiTank.gameObject).InitialBuildMinutes = _initialBuildMinutes;
                Get<TankInfo>.From(_aiTank.gameObject).InitialBuildSeconds = _initialBuildSeconds;
                Get<TankInfo>.From(_aiTank.gameObject).RequiredItems = _requiredItems;
                Get<TankInfo>.From(_aiTank.gameObject).AvailableInLevel = _availableInLevel;
            }
        }
    }
}
