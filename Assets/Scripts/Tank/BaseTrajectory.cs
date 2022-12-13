using UnityEngine;

public class BaseTrajectory : MonoBehaviour
{
    public virtual void PredictedTrajectory(float force)
    {

    }

    public virtual Vector3 PredictedTrajectory(Vector3 target, Vector3 origin, float time)
    {
        return Vector3.zero;
    }

    public virtual void UpdateTrajectoryTrace(bool isResetted)
    {

    }

    public virtual void PointsOverlapSphere(bool isLocalPlayer)
    {

    }
}
