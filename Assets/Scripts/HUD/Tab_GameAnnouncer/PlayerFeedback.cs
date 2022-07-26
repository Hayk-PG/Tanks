using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFeedback : BaseAnnouncer
{
    private HealthController _playerHealthController;
    private ScoreController _scoreController;


    protected override void OnDisable()
    {
        if (_playerHealthController != null) _playerHealthController.OnTakeDamage -= OnTakeDamage;
        if (_scoreController != null) _scoreController.OnHitEnemy -= OnGetPoints;
    }

    public void CallPlayerEvents(HealthController playerHealth, ScoreController scoreController)
    {
        _playerHealthController = playerHealth;
        _scoreController = scoreController;

        if (_playerHealthController != null) _playerHealthController.OnTakeDamage += OnTakeDamage;
        if (_scoreController != null) _scoreController.OnHitEnemy += OnGetPoints;
    }

    private void OnTakeDamage(BasePlayer basePlayer, int damage)
    {
        if (basePlayer != null)
        {
            //_playerDamageScreenText.Display(-damage);
        }        
    }

    private void OnGetPoints(int[] scoreValues)
    {
        StartCoroutine(OnGetPointsCoroutine());
    }

    private IEnumerator OnGetPointsCoroutine()
    {
        TextAnnouncement(0, "Good Hit!", true);
        SoundController.MusicSRCVolume(SoundController.MusicVolume.Down);
        SoundController.PlaySound(0, 2, out float clipLength);
        yield return new WaitForSeconds(clipLength);
        TextAnnouncement(0, "", false);
        SoundController.MusicSRCVolume(SoundController.MusicVolume.Up);
    }
}
