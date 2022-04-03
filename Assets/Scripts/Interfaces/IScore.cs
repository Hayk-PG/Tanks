using UnityEngine;
using System;

public interface IScore
{
    int Score { get; set; }

    IDamage IDamage { get; set; }
    PlayerTurn PlayerTurn { get; set; }

    Action<int, float> OnDisplayTempPoints { get; set; }
    Action<int> OnPlayerGetsPoints { get; set; }
    Action<int> OnHitEnemy { get; set; }

    void GetScore(int score, IDamage iDamage);
    void HitEnemyAndGetScore(int score, IDamage enemyDamage);
}
