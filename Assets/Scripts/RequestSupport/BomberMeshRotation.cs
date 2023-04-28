using UnityEngine;

public class BomberMeshRotation : MonoBehaviour
{
    [SerializeField]
    private Transform _transform;

    private float _verticalRotationSpeedLimit = 5;
    private float _time, _elapsedTime = 1;

    private Vector3 _rotationSpeed;





    private void Update()
    {
        _time += Time.deltaTime;

        if(_time >= _elapsedTime)
        {
            _rotationSpeed = new Vector3(0, Random.Range(-_verticalRotationSpeedLimit, _verticalRotationSpeedLimit), 0);

            _time = 0;

            _elapsedTime = Random.Range(0.1f, 1.1f);
        }

        transform.localEulerAngles += _rotationSpeed * Time.deltaTime;
    }
}
