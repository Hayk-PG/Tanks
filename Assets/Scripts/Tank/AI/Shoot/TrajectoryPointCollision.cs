using System.Collections;
using UnityEngine;

public class TrajectoryPointCollision : MonoBehaviour
{
    Transform _parent;
    TrajectoryPointCollision[] _trajectoryPointCollision;

    int _myIndex;
    int _previousPoint;

    public SpriteRenderer SpriteRenderer { get; set; }
    public bool IsEntered { get; set; }

    

    void Awake()
    {
        _parent = transform.parent;
        _myIndex = transform.GetSiblingIndex();
        SpriteRenderer = GetComponent<SpriteRenderer>();

        _trajectoryPointCollision = new TrajectoryPointCollision[_parent.childCount];

        for (int i = 0; i < _trajectoryPointCollision.Length; i++)
        {
            _trajectoryPointCollision[i] = _parent.GetChild(i).GetComponent<TrajectoryPointCollision>();
        }
    }

    void Start()
    {
        StartCoroutine(PointsVisibility());
    }

    IEnumerator PointsVisibility()
    {        
        while (true)
        {
            _previousPoint = 0;

            if (IsEntered == false && _myIndex > 0)
            {
                for (int i = 0; i < _myIndex; i++)
                {
                    if (_trajectoryPointCollision[i].IsEntered == false) _previousPoint++;
                }

                SpriteRenderer.enabled = _previousPoint == _myIndex ? true : false; 
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger)
        {
            IsEntered = true;

            for (int i = _myIndex; i < _trajectoryPointCollision.Length; i++)
            {
                _trajectoryPointCollision[i].SpriteRenderer.enabled = false;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.isTrigger) IsEntered = false;
    }
}
