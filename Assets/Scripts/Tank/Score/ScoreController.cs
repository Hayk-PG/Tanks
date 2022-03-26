using System;
using System.Linq;
using UnityEngine;

public class ScoreController : MonoBehaviour, IScore
{
    [SerializeField]
    private int _score;

    public PlayerTurn PlayerTurn { get; set; }
    public IDamage IDamage { get; set; }

    public int Score
    {
        get => _score;
        set => _score = value;
    }
    public Action<int, Vector3> OnDisplayTempPoints { get; set; }

    private IGetPlayerPoints[] _iGetPlayerPoints;


    private void Awake()
    {       
        IDamage = Get<IDamage>.From(gameObject);
        PlayerTurn = Get<PlayerTurn>.From(gameObject);

        Score = 0;

        SubscribeToIGetPlayerPointsEvent();
    }

    private void OnDisable()
    {
        UnsubscribeIGetPlayerPointsEvents();
    }
 
    public void GetScore(int score, IDamage iDamage, Vector3 position)
    {
        Conditions<bool>.Compare(iDamage != IDamage || iDamage == null, () => UpdateScore(score, position), null);
    }

    private void UpdateScore(int score, Vector3 position)
    {
        Score += score;
        OnDisplayTempPoints?.Invoke(score, position);
    }

    private void SubscribeToIGetPlayerPointsEvent()
    {
        _iGetPlayerPoints = FindObjectsOfType<MonoBehaviour>().OfType<IGetPlayerPoints>().ToArray();

        if(_iGetPlayerPoints != null)
        {
            foreach (var points in _iGetPlayerPoints)
            {
                points.OnGetPlayerPointsCount += OnIGetPlayerPointsCount;
            }
        }
    }

    private void UnsubscribeIGetPlayerPointsEvents()
    {
        if (_iGetPlayerPoints != null)
        {
            foreach (var points in _iGetPlayerPoints)
            {
                points.OnGetPlayerPointsCount -= OnIGetPlayerPointsCount;
            }
        }
    }

    private void OnIGetPlayerPointsCount(Action<int> OnMyPoints)
    {
        OnMyPoints?.Invoke(Score);
    }
}
