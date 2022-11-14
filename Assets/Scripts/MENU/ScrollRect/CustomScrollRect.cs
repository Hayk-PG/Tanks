using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]

public class CustomScrollRect : MonoBehaviour, IReset
{
    public ScrollRect _scrollRect;

    private RectTransform _rectTransform;
    private RectTransform _rectTransformContent;

    public bool IsContentOutside
    {
        get
        {
            return _rectTransformContent.rect.height - _rectTransform.rect.height >= 50;
        }
    }

    public event Action<bool, float> onValueChanged;


    private void Awake()
    {
        _scrollRect = Get<ScrollRect>.From(gameObject);

        _rectTransform = Get<RectTransform>.From(gameObject);
        _rectTransformContent = _scrollRect.content;
    }

    private void Update()
    {
        _scrollRect.onValueChanged.RemoveAllListeners();
        _scrollRect.onValueChanged.AddListener(OnValueChanged);
    }

    public void OnValueChanged(Vector2 position)
    {
        if (position.y > 1)
            _scrollRect.verticalNormalizedPosition = 1;

        if (position.y < 0)
            _scrollRect.verticalNormalizedPosition = 0;

        onValueChanged?.Invoke(IsContentOutside, position.y);

        print(_scrollRect.verticalNormalizedPosition);
    }

    public void SetNormalizedPosition(float position)
    {
        StartCoroutine(SetNormalizedPositionCoroutine(position));
    }

    private IEnumerator SetNormalizedPositionCoroutine(float position)
    {
        yield return null;
        _scrollRect.verticalNormalizedPosition = position;
    }

    public void SetDefault()
    {
        SetNormalizedPosition(1);
    }
}
