using UnityEngine;
using System;

public interface IScore
{
    int Score { get; set; }
    int MainScore { get; set; }

    IDamage IDamage { get; set; }
    PlayerTurn PlayerTurn { get; set; }

    public event Action<int, float> onDisplayPlayerScore;
    Action<int> OnPlayerGetsPoints { get; set; }
    Action<int[]> OnHitEnemy { get; set; }

    void GetScore(int score, IDamage iDamage, Vector3? visualPointsStartPosition = null, Vector3? targetPosition = null, bool convertStartPositionToScreenSpace = true, bool convertTargetPositionToScreenSpace = true);
    void HitEnemyAndGetScore(int[] scores, IDamage enemyDamage);
}
