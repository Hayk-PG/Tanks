using UnityEngine;

public class HUDScore : MonoBehaviour
{
    [SerializeField] private ScoreText _scoreText;
    [SerializeField] private RectTransform _tempPointsStartPoint;
    private TempPoints _tempPoints;

    public RectTransform TempPointsStartPoint
    {
        get => _tempPointsStartPoint;
    }
    public Transform ScoreTextTransform => _scoreText.transform;
    public CanvasGroup CanvasGroup { get; private set; }


    private void Awake()
    {
        CanvasGroup = Get<CanvasGroup>.From(gameObject);
        _tempPoints = FindObjectOfType<TempPoints>();
    }

    private void OnEnable() => _tempPoints.OnScoreTextUpdated += UpdateScore;

    private void OnDisable() => _tempPoints.OnScoreTextUpdated -= UpdateScore;

    public void UpdateScore(int score)
    {
        _scoreText.UpdateScoreText(score);
    }
}
