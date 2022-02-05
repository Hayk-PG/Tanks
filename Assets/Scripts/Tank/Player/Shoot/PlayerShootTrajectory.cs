using UnityEngine;

public class PlayerShootTrajectory : MonoBehaviour
{
    [SerializeField] GameObject pointsPrefab;
    [SerializeField] int numberOfPoints;

    GameObject[] points;


    void Awake()
    {
        InstantiatePoints();
    }

    void InstantiatePoints()
    {
        points = new GameObject[numberOfPoints];

        for (int i = 0; i < points.Length; i++)
        {
            int index = i;
            points[index] = Instantiate(pointsPrefab, transform.position, Quaternion.identity, transform);

            GraduallyResizingPoints(index);
        }
    }

    void GraduallyResizingPoints(int index)
    {
        float s = points[0].transform.localScale.x / numberOfPoints;

        if(index > 0) PointsScale(index, s);
    }

    void PointsScale(int index, float s)
    {
        points[index].transform.localScale = new Vector3(points[index - 1].transform.localScale.x - s, points[index - 1].transform.localScale.y - s, points[index - 1].transform.localScale.z - s);
    }

    internal Vector3 PointPosition(Vector3 direction, float force, float t)
    {
        Vector3 pointPos = transform.position + (direction.normalized * force * t) + 0.5f * Physics.gravity * (t * t);
        return pointPos;
    }

    internal void TrajectoryPrediction(float force)
    {
        for (int i = 0; i < points.Length; i++)
        {
            float dist = i * 0.02f;
            points[i].transform.position = PointPosition(transform.forward, force, dist);
        }
    }
}
