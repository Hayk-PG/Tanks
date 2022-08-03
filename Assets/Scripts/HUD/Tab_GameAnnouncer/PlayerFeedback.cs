using System.Collections;
using UnityEngine;


public class PlayerFeedback : BaseAnnouncer
{
    private HealthController _playerHealthController;
    private ScoreController _scoreController;
    private PlayerTurn _playerTurn;

    private int _playerHitsIndex;
    private int _playerTurnIndex;


    protected override void OnDisable()
    {
        base.OnDisable();

        if (_playerHealthController != null) _playerHealthController.OnTakeDamage -= OnTakeDamage;
        if (_scoreController != null) _scoreController.OnHitEnemy -= OnGetPoints;
    }

    public void CallPlayerEvents(HealthController playerHealth, ScoreController scoreController, PlayerTurn playerTurn)
    {
        _playerHealthController = playerHealth;
        _scoreController = scoreController;
        _playerTurn = playerTurn;

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
        StartCoroutine(OnGetPointsCoroutine(scoreValues));
    }

    private void GetComboScore(int total, int index)
    {
        float a = _soundController.SoundsList[2]._clips[index]._score;
        float b = a * 0.1f;
        float c = (total * b);

        _scoreController.GetScore(Mathf.RoundToInt(c), null);
    }

    private IEnumerator OnGetPointsCoroutine(int[] scoreValues)
    {
        _playerHitsIndex = _playerTurnIndex + 1;

        int total = 0;
        int index = 0;

        GlobalFunctions.Loop<int>.Foreach(scoreValues, value => { total += value; });

        if (_playerHitsIndex < 3)
        {
            for (int i = 0; i < _soundController.SoundsList[1]._clips.Length; i++)
            {
                if(_soundController.SoundsList[1]._clips[i]._score >= total)
                {
                    index = i;                   
                    break;
                }
            }

            yield return null;

            TextAnnouncement(0, _soundController.SoundsList[1]._clips[index]._clipName, true);
            SoundController.MusicSRCVolume(SoundController.MusicVolume.Down);
            SoundController.PlaySound(1, index, out float clipLength);
            yield return new WaitForSeconds(clipLength);
            TextAnnouncement(0, "", false);
            SoundController.MusicSRCVolume(SoundController.MusicVolume.Up);
        }

        else
        {
            for (int i = 0; i < _soundController.SoundsList[2]._clips.Length; i++)
            {
                if (_soundController.SoundsList[2]._clips[i]._score >= _playerHitsIndex)
                {
                    index = i;
                    break;
                }
            }

            yield return null;

            GetComboScore(total, index);
            TextAnnouncement(0, _soundController.SoundsList[2]._clips[index]._clipName, true);
            SoundController.MusicSRCVolume(SoundController.MusicVolume.Down);
            SoundController.PlaySound(2, index, out float clipLength);
            yield return new WaitForSeconds(clipLength);
            TextAnnouncement(0, "", false);
            SoundController.MusicSRCVolume(SoundController.MusicVolume.Up);
        }      
    }

    protected override void OnTurnChanged(TurnState turnState)
    {
        if(turnState == _playerTurn.MyTurn)
        {
            _playerTurnIndex++;

            if (_playerTurnIndex > _playerHitsIndex)
            {
                _playerTurnIndex = 0;
                _playerHitsIndex = 0;
            }
        }
    }
}
