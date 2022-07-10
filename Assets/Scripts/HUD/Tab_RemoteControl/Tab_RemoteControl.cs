using System;
using UnityEngine;

public class Tab_RemoteControl : MonoBehaviour
{
    private CanvasGroup _canvasGroup;

    [SerializeField] private RectTransform _screenRT;
    internal Button_TargetAnchoredPosition[] _targets;

    internal float _screenRTWidth, _screenRTHeight;
    internal float _minX, _maxX, _minY, _maxY;

    internal Action OnInitialize { get; set; }
    internal Action OnAnchoresPositionSet { get; set; }
    public Action<Vector3> OnGiveCoordinates { get; set; }

    private GameManager _gameManager;
    private Transform[] _tankCoordinates;


    private void Awake()
    {
        _canvasGroup = Get<CanvasGroup>.From(gameObject);

        _screenRTWidth = _screenRT.rect.width;
        _screenRTHeight = _screenRT.rect.height;
        _minX = -((_screenRTWidth / 2) - 100);
        _maxX = (_screenRTWidth / 2) - 100;
        _minY = -((_screenRTHeight / 2) - 100);
        _maxY = (_screenRTHeight / 2) - 100;

        _gameManager = FindObjectOfType<GameManager>();
    }

    private void OnEnable()
    {
        _gameManager.OnGameStarted += OnGameStarted;
    }

    private void OnDisable()
    {
        _gameManager.OnGameStarted -= OnGameStarted;
        GlobalFunctions.Loop<Button_TargetAnchoredPosition>.Foreach(_targets, target => { target.OnGiveCoordinates -= GiveCoordinates; });
    }

    private void OnGameStarted()
    {
        GetTanksTransforms();
        Initialize();
        Set();
    }

    private void GetTanksTransforms()
    {
        _tankCoordinates = new Transform[2] { _gameManager.Tank1.transform, _gameManager.Tank2.transform };
    }

    private void Initialize()
    {
        _targets = new Button_TargetAnchoredPosition[_screenRT.childCount];

        for (int i = 0; i < _screenRT.childCount; i++)
        {
            _targets[i] = Get<Button_TargetAnchoredPosition>.From(_screenRT.GetChild(i).gameObject);
            _targets[i].OnGiveCoordinates += GiveCoordinates;
        }

        OnInitialize?.Invoke();
    }

    private void Set()
    {
        for (int i = 0; i < _targets.Length; i++)
        {
            float x = UnityEngine.Random.Range(_minX, _maxX);
            float y = UnityEngine.Random.Range(_minY, _maxY);

            SetTargetsAnchoredPositions(i, new Vector2(x, y));
            SetTargetsCoordinates(i);
        }

        OnAnchoresPositionSet?.Invoke();
    } 
    
    private void SetTargetsAnchoredPositions(int i, Vector2 anchoredPosition)
    {
        _targets[i].SetAnchoredPosition(anchoredPosition);
    }

    private void SetTargetsCoordinates(int i)
    {
        if (i < 2)
        {
            _targets[i].SetCoordinates(_tankCoordinates[i]);
        }
    }

    private void GiveCoordinates(Transform coordinate)
    {
        if (coordinate != null)
        {
            OnGiveCoordinates?.Invoke(coordinate.position);
        }
        else
        {
            int randomIndex = UnityEngine.Random.Range(0, 1);
            float randomX = UnityEngine.Random.Range(5, 10);
            Vector3 pos = _tankCoordinates[randomIndex].position;

            if (pos == _tankCoordinates[0].position)
                OnGiveCoordinates?.Invoke(new Vector3(pos.x + randomX, pos.y, pos.z));
            else
                OnGiveCoordinates?.Invoke(new Vector3(pos.x - randomX, pos.y, pos.z));
        }

        GlobalFunctions.CanvasGroupActivity(_canvasGroup, false);
    }
}
