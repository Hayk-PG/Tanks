using System;
using UnityEngine;

public class EnemyPlayerIcon : MonoBehaviour
{
    private RectTransform _rectTransform;
    public Transform EnemyPlayer { get; set; }

    private Vector2 _size;
    private Vector2 _iconMovementPosition;
    private Vector3 _iconInitialWorldPosition;

    private struct AnchorPresets
    {
        internal AnchorPresets(EnemyPlayerIcon EnemyPlayerIcon, float minX, float maxX)
        {
            EnemyPlayerIcon._rectTransform.anchorMin = new Vector2(minX, 0.5f);
            EnemyPlayerIcon._rectTransform.anchorMax = new Vector2(maxX, 0.5f);
        }
    }//Not needed
    private struct StartDirectionAndPosition
    {
        internal StartDirectionAndPosition(EnemyPlayerIcon enemyPlayerIcon, float scaleX, float anchoredPositionX)
        {
            enemyPlayerIcon._rectTransform.localScale = new Vector3(scaleX, 1, 1);
            enemyPlayerIcon._rectTransform.anchoredPosition = new Vector2(anchoredPositionX, 0);
        }
    }

    private CanvasGroup _canvasGroup;
    private HUDBounds _hudBounds;

    public event Action<Action> OnIconMove;



    private void Awake()
    {
        _rectTransform = Get<RectTransform>.From(gameObject);
        _canvasGroup = Get<CanvasGroup>.From(gameObject);
        _hudBounds = Get<HUDBounds>.From(gameObject);
    }

    private void Start()
    {
        _size = new Vector2(_rectTransform.sizeDelta.x, _rectTransform.sizeDelta.y);
    }

    private void Update()
    {
        OnIconMove?.Invoke(IconMovement);       
    }

    public void SetInitialCoordinates(bool isPlayerOne, Transform enemyPlayer)
    {
        float anchorMinX = isPlayerOne ? 1 : 0;
        float anchorMaxX = isPlayerOne ? 1 : 0;
        float scaleX = isPlayerOne ? 1 : -1;
        float anchoredPositionX = isPlayerOne ? -_size.x : _size.x;

        StartDirectionAndPosition StartDirectionAndPosition = new StartDirectionAndPosition(this, scaleX, anchoredPositionX);
        _iconInitialWorldPosition = transform.position;

        EnemyPlayer = enemyPlayer;
    }

    public void IconMovement()
    {
        if (EnemyPlayer != null)
        {
            _iconMovementPosition = CameraSight.ScreenPoint(EnemyPlayer.position);
            float x = Mathf.Clamp(_iconMovementPosition.x, -_hudBounds._canvasPixelRectMax.x + _size.x, _hudBounds._canvasPixelRectMax.x - _size.x);
            float y = Mathf.Clamp(_iconMovementPosition.y, -_hudBounds._canvasPixelRectMax.y + _size.y, _hudBounds._canvasPixelRectMax.y - _size.y);


            transform.position = new Vector2(x, y);
            ////GlobalFunctions.CanvasGroupActivity(_canvasGroup, !CameraSight.IsInCameraSight(EnemyPlayer.position));
        }
    }
}
