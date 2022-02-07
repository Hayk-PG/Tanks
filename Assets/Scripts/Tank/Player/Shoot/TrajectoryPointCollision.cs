using System.Collections;
using UnityEngine;

public class TrajectoryPointCollision : MonoBehaviour
{
    private Transform _parent;
    private TrajectoryPointCollision[] _trajectoryPointCollision;

    private int _myIndex;
    private int _previousPoint;
    private GameObject _collisionGameObject;


    public SpriteRenderer SpriteRenderer { get; set; }
    public bool IsEntered { get; set; }



    private void Awake()
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

    private void Start()
    {
        StartCoroutine(PointsVisibility());
    }

    private IEnumerator PointsVisibility()
    {        
        while (true)
        {
            _previousPoint = 0;

            if (IsEntered == false && _myIndex > 0)
            {
                for (int i = 0; i < _myIndex; i++)
                {
                    if (_trajectoryPointCollision[i].IsEntered == false && _trajectoryPointCollision[i]._collisionGameObject == null) _previousPoint++;
                }

                SpriteRenderer.enabled = _previousPoint == _myIndex ? true : false; 
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger)
        {
            IsEntered = true;
            if(_collisionGameObject != other.gameObject) _collisionGameObject = other.gameObject;

            for (int i = _myIndex; i < _trajectoryPointCollision.Length; i++)
            {
                _trajectoryPointCollision[i].SpriteRenderer.enabled = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.isTrigger)
        {
            IsEntered = false;
            if (_collisionGameObject != null) _collisionGameObject = null;
        }
    }
}
