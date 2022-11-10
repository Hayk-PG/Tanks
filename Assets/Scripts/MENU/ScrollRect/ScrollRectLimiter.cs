using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]

public class ScrollRectLimiter : MonoBehaviour
{
    private ScrollRect _scrollRect;


    private void Awake()
    {
        _scrollRect = Get<ScrollRect>.From(gameObject);    
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
    }
}
