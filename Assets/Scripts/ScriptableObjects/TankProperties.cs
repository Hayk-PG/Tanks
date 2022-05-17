using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New tank", menuName = "Tank")]
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
    public float _accelerated;
    public float _maxBrake;
    public Vector3 _normalCenterOfMass;

    [Header("Tank prefab canon parameters")]
    public float _minEulerAngleX;
    public float _maxEulerAngleX;
    public float _rotationSpeed;
    public Vector3 _rotationStabilizer;

    [Header("Shoot force")]
    public float _minForce;
    public float _maxForce;
    public float _smoothTime;
    public float _maxSpeed;

    [Header("Health")]
    public int _armor;


    public virtual void GetValuesFromTankPrefab()
    {
        _rigidbodyMass = Get<Rigidbody>.From(_tank.gameObject).mass; 
        _normalSpeed = Get<BaseTankMovement>.From(_tank.gameObject)._normalSpeed;
        _accelerated = Get<BaseTankMovement>.From(_tank.gameObject)._accelerated;
        _maxBrake = Get<BaseTankMovement>.From(_tank.gameObject)._maxBrake;
        _normalCenterOfMass = Get<BaseTankMovement>.From(_tank.gameObject)._normalCenterOfMass;

        _minEulerAngleX = Get<BaseShootController>.From(_tank.gameObject)._canon._minEulerAngleX;
        _maxEulerAngleX = Get<BaseShootController>.From(_tank.gameObject)._canon._maxEulerAngleX;
        _rotationSpeed = Get<BaseShootController>.From(_tank.gameObject)._canon._rotationSpeed;
        _rotationStabilizer = Get<BaseShootController>.From(_tank.gameObject)._canon._rotationStabilizer;

        _minForce = Get<BaseShootController>.From(_tank.gameObject)._shoot._minForce;
        _maxForce = Get<BaseShootController>.From(_tank.gameObject)._shoot._maxForce;
        _smoothTime = Get<BaseShootController>.From(_tank.gameObject)._shoot._smoothTime;
        _maxSpeed = Get<BaseShootController>.From(_tank.gameObject)._shoot._maxSpeed;

        _armor = Get<HealthController>.From(_tank.gameObject).Armor;

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

                Get<BaseShootController>.From(_aiTank.gameObject)._shoot._minForce = _minForce;
                Get<BaseShootController>.From(_aiTank.gameObject)._shoot._maxForce = _maxForce;
                Get<BaseShootController>.From(_aiTank.gameObject)._shoot._smoothTime = _smoothTime;
                Get<BaseShootController>.From(_aiTank.gameObject)._shoot._maxSpeed = _maxSpeed;
            }

            if (Get<HealthController>.From(_aiTank.gameObject) != null)
            {
                Get<HealthController>.From(_aiTank.gameObject).Armor = _armor;
            }
        }
    }
}
