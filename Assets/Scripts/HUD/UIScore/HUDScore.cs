using UnityEngine;

public class HUDScore : MonoBehaviour
{
    /// <summary>
    /// 0: Player 1
    /// 1: Player 2
    /// </summary>
    [SerializeField]
    private ScoreText[] _scores;


    /// <summary>
    /// 0: Player 1
    /// 1: Player 2
    /// </summary>
    public void UpdateScore(int score, int scoreIndex)
    {
        _scores[scoreIndex].UpdateScoreText(score);
    }
}
