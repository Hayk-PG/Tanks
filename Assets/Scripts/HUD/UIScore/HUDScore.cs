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

    public Transform ScoreTextTransform { get => _scoreText.transform; }


    private void Awake()
    {
        _tempPoints = FindObjectOfType<TempPoints>();
    }

    private void OnEnable()
    {
        _tempPoints.OnUpdateScore += UpdateScore;
    }

    private void OnDisable()
    {
        _tempPoints.OnUpdateScore -= UpdateScore;
    }

    public void UpdateScore(int score)
    {
        _scoreText.UpdateScoreText(score);
    }
}
