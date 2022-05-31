using UnityEngine;

public class TopBarResourcesContainer : MonoBehaviour
{
    private RectTransform _titleRectTransform;
    private Vector2 _defaultAnchorMax;
    private Vector2 _defaultOffsetMax;
    private CanvasGroup _buttonsContainer;

    private void OnEnable()
    {
        if (_titleRectTransform == null)
        {
            _titleRectTransform = transform.parent.Find("Title").GetComponent<RectTransform>();
            _buttonsContainer = transform.parent.Find("ButtonsContainer").GetComponent<CanvasGroup>();
            _defaultAnchorMax = _titleRectTransform.anchorMax;
            _defaultOffsetMax = _titleRectTransform.offsetMax;
            
        }

        GlobalFunctions.CanvasGroupActivity(_buttonsContainer, false);
        _titleRectTransform.anchorMax = new Vector2(0.2f, _titleRectTransform.anchorMax.y);
        _titleRectTransform.offsetMax = Vector2.zero;        
    }

    private void OnDisable()
    {
        GlobalFunctions.CanvasGroupActivity(_buttonsContainer, true);
        _titleRectTransform.anchorMax = _defaultAnchorMax;
        _titleRectTransform.offsetMax = _defaultOffsetMax;       
    }
}
