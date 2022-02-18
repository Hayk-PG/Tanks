using UnityEngine;

public class ScoreController : MonoBehaviour, IScore
{
    [SerializeField]
    private int _score;

    private HUDScore _hudScore;
    private PlayerTurn _playerTurn;
    public IDamage IDamage { get; set; }

    public int Score
    {
        get => _score;
        set => _score = value;
    }
    

    private void Awake()
    {       
        _hudScore = FindObjectOfType<HUDScore>();
        IDamage = Get<IDamage>.From(gameObject);
        _playerTurn = Get<PlayerTurn>.From(gameObject);

        Score = 0;
    }
  
    public void GetScore(int score, IDamage iDamage)
    {
        Conditions<bool>.Compare(iDamage != IDamage || iDamage == null, () => OnUpdateScore(score), null);
    }

    private void OnUpdateScore(int score)
    {
        Score += score;
        _hudScore.UpdateScore(Score, ScoreIndex());
    }

    private int ScoreIndex()
    {
        return _playerTurn.MyTurn == TurnState.Player1 ? 0 : 1;
    } 
}
