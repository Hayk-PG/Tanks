using System;
using System.Collections;
using UnityEngine;

public class Button_TargetAnchoredPosition : MonoBehaviour
{
    [SerializeField] private Tab_RemoteControl _tabRemoteControl;
    private Button_TargetAnchoredPosition[] _otherTargets;
    private RectTransform _rectTransform;
    private int loopCount;

    public Transform Coordinates { get; set; }
    public Action<Transform> OnGiveCoordinates { get; set; }



    private void Awake()
    {
        _rectTransform = Get<RectTransform>.From(gameObject);       
    }

    private void OnEnable()
    {
        _tabRemoteControl.OnInitialize += GetOtherTargets;
        _tabRemoteControl.OnAnchoresPositionSet += StartUpdateUnchoredPositionCoroutine;
    }

    private void OnDisable()
    {
        _tabRemoteControl.OnInitialize -= GetOtherTargets;
        _tabRemoteControl.OnAnchoresPositionSet -= StartUpdateUnchoredPositionCoroutine;
    }

    internal void SetAnchoredPosition(Vector2 pos)
    {
        _rectTransform.anchoredPosition = pos;
    }

    internal void SetCoordinates(Transform coordinates)
    {
        Coordinates = coordinates;
    }

    private void GetOtherTargets()
    {
        if (_tabRemoteControl != null)
        {
            _otherTargets = new Button_TargetAnchoredPosition[_tabRemoteControl._targets.Length - 1];
            int index = 0;

            foreach (var target in _tabRemoteControl._targets)
            {
                if (target == this)
                {
                    continue;
                }

                _otherTargets[index] = target;
                index++;
            }
        }
    }

    private IEnumerator UpdateUnchoredPosition()
    {
        while (loopCount < 10)
        {
            foreach (var other in _otherTargets)
            {
                if (_rectTransform.anchoredPosition.x <= other._rectTransform.anchoredPosition.x + 100 && _rectTransform.anchoredPosition.x >= other._rectTransform.anchoredPosition.x && _rectTransform.anchoredPosition.y <= other._rectTransform.anchoredPosition.y + 100 && _rectTransform.anchoredPosition.y >= other._rectTransform.anchoredPosition.y)
                {
                    _rectTransform.anchoredPosition = new Vector2(other._rectTransform.anchoredPosition.x + 100, _rectTransform.anchoredPosition.y + 100);
                }

                else if (_rectTransform.anchoredPosition.x >= other._rectTransform.anchoredPosition.x - 100 && _rectTransform.anchoredPosition.x <= other._rectTransform.anchoredPosition.x && _rectTransform.anchoredPosition.y <= other._rectTransform.anchoredPosition.y + 100 && _rectTransform.anchoredPosition.y >= other._rectTransform.anchoredPosition.y)
                {
                    _rectTransform.anchoredPosition = new Vector2(other._rectTransform.anchoredPosition.x - 100, _rectTransform.anchoredPosition.y + 100);
                }

                else if (_rectTransform.anchoredPosition.x <= other._rectTransform.anchoredPosition.x + 100 && _rectTransform.anchoredPosition.x >= other._rectTransform.anchoredPosition.x && _rectTransform.anchoredPosition.y >= other._rectTransform.anchoredPosition.y - 100 && _rectTransform.anchoredPosition.y <= other._rectTransform.anchoredPosition.y)
                {
                    _rectTransform.anchoredPosition = new Vector2(other._rectTransform.anchoredPosition.x + 100, _rectTransform.anchoredPosition.y - 100);
                }

                else if (_rectTransform.anchoredPosition.x >= other._rectTransform.anchoredPosition.x - 100 && _rectTransform.anchoredPosition.x <= other._rectTransform.anchoredPosition.x && _rectTransform.anchoredPosition.y >= other._rectTransform.anchoredPosition.y - 100 && _rectTransform.anchoredPosition.y <= other._rectTransform.anchoredPosition.y)
                {
                    _rectTransform.anchoredPosition = new Vector2(other._rectTransform.anchoredPosition.x - 100, _rectTransform.anchoredPosition.y - 100);
                }

                else if (_rectTransform.anchoredPosition.x <= _tabRemoteControl._minX || _rectTransform.anchoredPosition.x >= _tabRemoteControl._maxX || _rectTransform.anchoredPosition.y <= _tabRemoteControl._minY || _rectTransform.anchoredPosition.y >= _tabRemoteControl._maxY)
                {
                    _rectTransform.anchoredPosition = Vector2.zero;
                }      
            }

            loopCount++;
            yield return new WaitForSeconds(1);
        }
    }

    private void StartUpdateUnchoredPositionCoroutine()
    {
        StartCoroutine(UpdateUnchoredPosition());
    }
}
