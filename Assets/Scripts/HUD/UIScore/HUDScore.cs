using System;
using UnityEngine;

public class HUDScore : MonoBehaviour
{
    /// <summary>
    /// 0:Player1
    /// 1:Player2
    /// </summary>
    [SerializeField]
    private ScoreText[] _scores;

    [Serializable]
    public struct ScoresTransform
    {
        public Vector2 _scorePosition;

        public ScoresTransform(Vector2 scorePosition)
        {
            _scorePosition = scorePosition;
        }
    }
    public ScoresTransform[] _scoresTransform;

    private TempPoints _tempPoints;


    private void Awake()
    {
        ScoresTransform scoreTr1 = new ScoresTransform(new Vector2(_scores[0].transform.position.x + _scores[0].GetComponent<RectTransform>().sizeDelta.x / 2, _scores[0].transform.position.y));
        ScoresTransform scoreTr2 = new ScoresTransform(new Vector2(_scores[1].transform.position.x - _scores[1].GetComponent<RectTransform>().sizeDelta.x / 2, _scores[1].transform.position.y));

        _scoresTransform = new ScoresTransform[2] { scoreTr1 , scoreTr2 };

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

    public void UpdateScore(int score, int scoreIndex)
    {
        _scores[scoreIndex].UpdateScoreText(score);
    }
}
