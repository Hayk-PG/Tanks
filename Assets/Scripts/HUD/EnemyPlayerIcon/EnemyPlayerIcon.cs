using System;
using UnityEngine;

public class EnemyPlayerIcon : MonoBehaviour
{
    private RectTransform _rectTransform;
    private Vector2 Size
    {
        get => _rectTransform.sizeDelta;
    }
    private Vector2 FaceDirection
    {
        get => _rectTransform.localScale;
        set => _rectTransform.localScale = value;
    }
    
    private struct Temp
    {
        internal Vector2 _iconMovementPosition;
        internal float _x, _y; 
    }
    private Temp _temp;

    private CanvasGroup _canvasGroup;
    private HUDBounds _hudBounds;

    public Transform EnemyPlayer { get; set; }

    public event Action<Action> OnIconMove;



    private void Awake()
    {
        _rectTransform = Get<RectTransform>.From(gameObject);
        _canvasGroup = Get<CanvasGroup>.From(gameObject);
        _hudBounds = Get<HUDBounds>.From(gameObject);
    }

    private void Update()
    {
        OnIconMove?.Invoke(IconMovement);       
    }

    public void SetInitialCoordinates(bool isPlayerOne, Transform enemyPlayer)
    {
        FaceDirection = isPlayerOne ? new Vector2(1, 1) : new Vector2(-1, 1);

        EnemyPlayer = enemyPlayer;
    }

    public void IconMovement()
    {
        if (EnemyPlayer == null) return;

        _temp._iconMovementPosition = CameraSight.ScreenPoint(EnemyPlayer.position);
        _temp._x = Mathf.Clamp(_temp._iconMovementPosition.x, -_hudBounds._canvasPixelRectMax.x + Size.x, _hudBounds._canvasPixelRectMax.x - Size.x);
        _temp._y = Mathf.Clamp(_temp._iconMovementPosition.y, -_hudBounds._canvasPixelRectMax.y + Size.y, _hudBounds._canvasPixelRectMax.y - Size.y);

        transform.position = new Vector2(_temp._x, _temp._y);
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, !CameraSight.IsInCameraSight(EnemyPlayer.position));
    }
}
