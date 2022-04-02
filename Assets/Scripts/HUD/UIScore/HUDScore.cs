using UnityEngine;

public class HUDScore : MonoBehaviour
{
    /// <summary>
    /// 0:Player1
    /// 1:Player2
    /// </summary>
    [SerializeField]
    private ScoreText _scoreText;
    private TempPoints _tempPoints;

    [SerializeField]
    private RectTransform _tempPointsStartPoint;
    public RectTransform TempPointsStartPoint
    {
        get => _tempPointsStartPoint;
    }

    public Transform ScoreTextTransform { get => _scoreText.transform; }


    private void Awake()
    {
        _tempPoints = FindObjectOfType<TempPoints>();
    }

    private void OnEnable()
    {
        _tempPoints.OnScoreTextUpdated += UpdateScore;
    }

    private void OnDisable()
    {
        _tempPoints.OnScoreTextUpdated -= UpdateScore;
    }

    public void UpdateScore(int score)
    {
        _scoreText.UpdateScoreText(score);
    }
}
