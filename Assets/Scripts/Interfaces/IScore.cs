using UnityEngine;
using System;

public interface IScore
{
    int Score { get; set; }

    IDamage IDamage { get; set; }
    PlayerTurn PlayerTurn { get; set; }

    Action<int, Vector3, float> OnDisplayTempPoints { get; set; }

    void GetScore(int score, IDamage iDamage, Vector3 explosionPosition);
}
