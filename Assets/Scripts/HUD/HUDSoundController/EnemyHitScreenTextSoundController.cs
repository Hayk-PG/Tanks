using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitScreenTextSoundController : UIBaseSoundController
{
    private EnemyHitScreenText _enemyHitScreenText;

    private void Awake()
    {
        _enemyHitScreenText = FindObjectOfType<EnemyHitScreenText>();
    }

    private void OnEnable()
    {
        _enemyHitScreenText.OnSoundFX += OnSoundFX;
    }

    private void OnDisable()
    {
        _enemyHitScreenText.OnSoundFX -= OnSoundFX;
    }

    private void OnSoundFX()
    {
        PlaySoundFX(0);
    }
}
