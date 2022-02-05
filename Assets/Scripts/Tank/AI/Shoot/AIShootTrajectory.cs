using UnityEngine;

public class AIShootTrajectory : MonoBehaviour
{
    Vector3 _distance, _distanceX, _result;
    float _sy, _sx, _vx, _vy;


    public Vector3 PredictedTrajectory(Vector3 target, Vector3 origin, float time)
    {
        _distance = target - origin;
        _distanceX = _distance;

        _sy = _distance.y;
        _sx = _distanceX.magnitude;

        _vx = _sx / time;
        _vy = _sy / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

        _result = _distanceX.normalized;
        _result *= _vx;
        _result.y = _vy;

        return _result;
    }
}
