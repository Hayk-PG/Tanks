﻿using System;
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
    public Action<int, TurnState, Vector3> OnDisplayTemPoints { get; set; }



    private void Awake()
    {       
        IDamage = Get<IDamage>.From(gameObject);
        PlayerTurn = Get<PlayerTurn>.From(gameObject);

        Score = 0;
    }
  
    public void GetScore(int score, IDamage iDamage, Vector3 position)
    {
        Conditions<bool>.Compare(iDamage != IDamage || iDamage == null, () => UpdateScore(score, position), null);
    }

    private void UpdateScore(int score, Vector3 position)
    {
        Score += score;
        OnDisplayTemPoints?.Invoke(score, PlayerTurn.MyTurn, position);
    }
}
