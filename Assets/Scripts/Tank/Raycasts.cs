using System;
using UnityEngine;

public class Raycasts : MonoBehaviour
{
    [Serializable] [SerializeField] struct RaycastPoints
    {
        [SerializeField] internal Transform frontRayStartPoint;
        [SerializeField] internal Transform backRayStartPoint;
        [SerializeField] internal Transform downRayStartPoint;
        
        internal RaycastHit frontHit;
        internal RaycastHit backHit;
        internal RaycastHit downHit;       
    }

    [SerializeField] RaycastPoints _RaycastPoints;

    internal RaycastHit FrontHit { get => _RaycastPoints.frontHit; }
    internal RaycastHit BackHit { get => _RaycastPoints.backHit; }
    internal RaycastHit DownHit { get => _RaycastPoints.downHit; }
    

    public void CastRays(Vector3 frontRayDirection, Vector3 backRayDirection)
    {
        Physics.Raycast(_RaycastPoints.frontRayStartPoint.position, frontRayDirection, out _RaycastPoints.frontHit, 0.2f);
        Physics.Raycast(_RaycastPoints.backRayStartPoint.position, backRayDirection, out _RaycastPoints.backHit, 0.2f);
        Physics.Raycast(_RaycastPoints.downRayStartPoint.position, Vector3.down, out _RaycastPoints.downHit, 0.2f);
        
        Debug.DrawRay(_RaycastPoints.frontRayStartPoint.position, frontRayDirection * 0.2f, Color.red);
        Debug.DrawRay(_RaycastPoints.backRayStartPoint.position, backRayDirection * 0.2f, Color.red);
        Debug.DrawRay(_RaycastPoints.downRayStartPoint.position, Vector3.down * 0.2f, Color.red);
    }
}
