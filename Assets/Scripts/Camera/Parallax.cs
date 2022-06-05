using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private float _parallaxEffectX, _parallaxEffectY;
    [SerializeField] private float _add;
    private float _startPos;
    private float _distX, _distY;

    

    private void Awake()
    {
        _startPos = transform.position.x;
    }

    private void FixedUpdate()
    {
        _distX = (_mainCamera.transform.position.x * _parallaxEffectX);
        _distY = (_mainCamera.transform.position.y * _parallaxEffectY);
        transform.position = new Vector3(_startPos - _distX, _startPos - _distY + _add, transform.position.z);
    }
}
