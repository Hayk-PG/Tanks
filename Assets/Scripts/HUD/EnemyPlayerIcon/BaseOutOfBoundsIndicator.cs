using UnityEngine;

public abstract class BaseOutOfBoundsIndicator : MonoBehaviour
{
    [SerializeField]
    protected CanvasGroup _canvasGroup;

    [SerializeField] [Space]
    protected RectTransform _rectTransform;

    [SerializeField] [Space]
    protected HUDBounds _hudBounds;

    protected float _xPosition, _yPosition;

    protected Vector2 _iconPosition;

    protected virtual Vector2 Size
    {
        get => _rectTransform.sizeDelta;
    }
    protected virtual Vector2 FaceDirection
    {
        get => _rectTransform.localScale;
        set => _rectTransform.localScale = value;
    }

    protected Transform Target { get; set; }




    public abstract void Init(Transform target);

    protected virtual void ControlMovement()
    {
        if (Target == null)
        {
            SetActive(null, true);

            return;
        }

        transform.position = DesiredPosition(Target);

        SetActive(Target);
    }

    protected virtual Vector2 DesiredPosition(Transform target)
    {
        _iconPosition = CameraSight.ScreenPoint(target.position);

        _xPosition = Mathf.Clamp(_iconPosition.x, _hudBounds._canvasPixelRectMin.x + Size.x, _hudBounds._canvasPixelRectMax.x - Size.x);
        _yPosition = Mathf.Clamp(_iconPosition.y, -_hudBounds._canvasPixelRectMin.y + Size.y, _hudBounds._canvasPixelRectMax.y - Size.y);

        return new Vector2(_xPosition, _yPosition);
    }

    protected virtual void SetActive(Transform target, bool forceClose = false)
    {
        if (forceClose)
        {
            if (_canvasGroup.alpha <= 0)
                return;

            GlobalFunctions.CanvasGroupActivity(_canvasGroup, false);

            _canvasGroup.blocksRaycasts = false;

            return;
        }

        GlobalFunctions.CanvasGroupActivity(_canvasGroup, !CameraSight.IsInCameraSight(target.position));

        _canvasGroup.blocksRaycasts = false;
    }
}
