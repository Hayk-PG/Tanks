using UnityEngine;

public class HUDScore : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup _canvasGroup;

    [SerializeField] [Space]
    private ScoreText _scoreText;

    [SerializeField] [Space]
    private RectTransform _tempPointsStartPoint;

    [SerializeField] [Space]
    private TempPoints _tempPoints;

    public CanvasGroup CanvasGroup => _canvasGroup;

    public RectTransform TempPointsStartPoint
    {
        get => _tempPointsStartPoint;
    }
    public Transform ScoreTextTransform => _scoreText.transform;




    private void OnEnable() => _tempPoints.OnScoreTextUpdated += UpdateScore;

    private void OnDisable() => _tempPoints.OnScoreTextUpdated -= UpdateScore;

    public void UpdateScore(int score)
    {
        _scoreText.UpdateScoreText(score);
    }
}
