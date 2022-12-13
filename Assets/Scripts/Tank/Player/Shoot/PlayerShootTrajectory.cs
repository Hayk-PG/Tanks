using UnityEngine;

public class PlayerShootTrajectory : BaseTrajectory
{
    [SerializeField]
    private GameObject pointsPrefab, _pointTarget;
    private GameObject[] points;
    [SerializeField] 
    private int numberOfPoints;   
    private SpriteRenderer[] _tracePointsSpriteRenderer;
    private GameObject _tracePointsParent;
    private Color _tracePointsColor = new Color(1, 1, 1, 0.3f);
    private Color _tracePointsResetColor = new Color(1, 1, 1, 0.0f);
    private Collider[] _colliders;


    private void Awake()
    {
        InstantiatePoints();
    }

    private void InstantiatePoints()
    {
        points = new GameObject[numberOfPoints];

        for (int i = 0; i < points.Length; i++)
        {
            int index = i;
            points[index] = Instantiate(pointsPrefab, transform.position, Quaternion.identity, transform);

            GraduallyResizingPoints(index);
        }

        InstantiateTrace();
    }

    private void InstantiateTrace()
    {
        _tracePointsParent = new GameObject("Trace");
        _tracePointsParent.transform.parent = transform;
        _tracePointsParent.transform.localPosition = Vector3.zero;
        _tracePointsParent.transform.localScale = new Vector3(1, 1, 1);
        _tracePointsSpriteRenderer = new SpriteRenderer[points.Length];

        for (int i = 0; i < points.Length; i++)
        {
            _tracePointsSpriteRenderer[i] = Instantiate(pointsPrefab.GetComponent<SpriteRenderer>(), points[i].transform.position, Quaternion.identity, _tracePointsParent.transform);
            _tracePointsSpriteRenderer[i].transform.localScale = points[i].transform.localScale;
            _tracePointsSpriteRenderer[i].color = _tracePointsColor;
        }
    }

    public override void UpdateTrajectoryTrace(bool isResetted)
    {
        if (!isResetted)
        {
            for (int i = 0; i < _tracePointsSpriteRenderer.Length; i++)
            {
                _tracePointsSpriteRenderer[i].transform.position = points[i].transform.position;
                _tracePointsSpriteRenderer[i].transform.localScale = points[i].transform.localScale;
                _tracePointsSpriteRenderer[i].color = _tracePointsColor;
            }
        }
        else
        {
            for (int i = 0; i < _tracePointsSpriteRenderer.Length; i++)
            {
                _tracePointsSpriteRenderer[i].color = _tracePointsResetColor;
            }
        }
    }

    private void GraduallyResizingPoints(int index)
    {
        float s = points[0].transform.localScale.x / numberOfPoints;

        if(index > 0) PointsScale(index, s);
    }

    private void PointsScale(int index, float s)
    {
        points[index].transform.localScale = new Vector3(points[index - 1].transform.localScale.x - s, points[index - 1].transform.localScale.y - s, points[index - 1].transform.localScale.z - s);
    }

    internal Vector3 PointPosition(Vector3 direction, float force, float t)
    {
        Vector3 pointPos = transform.position + (direction.normalized * force * t) + 0.5f * Physics.gravity * (t * t);
        return pointPos;
    }

    public override void PredictedTrajectory(float force)
    {
        for (int i = 0; i < points.Length; i++)
        {
            float dist = i * 0.02f;
            points[i].transform.position = PointPosition(transform.forward, force, dist);
        }
    }

    public override void PointsOverlapSphere(bool isLocalPlayer)
    {
        if (isLocalPlayer)
        {
            _pointTarget.SetActive(false);

            for (int i = 0; i < points.Length; i++)
            {
                _colliders = Physics.OverlapSphere(points[i].transform.position, 0.1f, 1, QueryTriggerInteraction.Ignore);

                if (_colliders.Length > 0)
                {
                    for (int x = i; x < points.Length; x++)
                    {
                        points[x].SetActive(false);
                    }

                    _pointTarget.SetActive(true);
                    _pointTarget.transform.position = _colliders[0].ClosestPoint(points[i].transform.position);
                    _pointTarget.transform.Rotate(Vector3.forward);
                    break;
                }
                else
                {
                    points[i].SetActive(true);
                }
            }
        }
    }
}
