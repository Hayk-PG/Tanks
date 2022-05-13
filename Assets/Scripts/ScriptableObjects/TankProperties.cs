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

    [Header("Tank prefab movement parameters")]
    public float _rigidbodyMass;
    public float _normalSpeed;
    public float _accelerated;
    public float _maxBrake;
    public Vector3 _centerOfMass;

    [Header("Tank prefab canon parameters")]
    public float _minEulerAngleX;
    public float _maxEulerAngleX;


    public virtual void GetValuesFromTankPrefab()
    {
        _rigidbodyMass = _tank.GetComponent<Rigidbody>().mass;
        _normalSpeed = _tank.GetComponent<TankMovement>()._normalSpeed;
        _accelerated = _tank.GetComponent<TankMovement>()._accelerated;
        _maxBrake = _tank.GetComponent<TankMovement>()._maxBrake;
        _centerOfMass = _tank.GetComponent<TankMovement>()._normalCenterOfMass;
        _minEulerAngleX = _tank.GetComponent<ShootController>()._canon._minEulerAngleX;
        _maxEulerAngleX = _tank.GetComponent<ShootController>()._canon._maxEulerAngleX;

        EditorUtility.SetDirty(this);
    }
}
