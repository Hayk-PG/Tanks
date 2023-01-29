using System.Collections;
using UnityEngine;


public class PlayerFeedback : BaseAnnouncer
{
    private HealthController _playerHealthController;
    private ScoreController _scoreController;
    private PlayerTurn _playerTurn;
    private HitTextManager _hitTextManager;

    private int _playerHitsIndex;
    private int _playerTurnIndex;



    protected override void OnDisable()
    {
        base.OnDisable();

        if (_playerHealthController != null) 
            _playerHealthController.OnTakeDamage -= OnTakeDamage;

        if (_scoreController != null) 
            _scoreController.OnHitEnemy -= OnHitEnemy;
    }

    public void CallPlayerEvents(HealthController playerHealth, ScoreController scoreController, PlayerTurn playerTurn)
    {
        _playerHealthController = playerHealth;
        _scoreController = scoreController;
        _playerTurn = playerTurn;
        StartCoroutine(GetHitTextManager(playerTurn));

        if (_playerHealthController != null) 
            _playerHealthController.OnTakeDamage += OnTakeDamage;

        if (_scoreController != null) 
            _scoreController.OnHitEnemy += OnHitEnemy;
    }

    private IEnumerator GetHitTextManager(PlayerTurn playerTurn)
    {
        yield return new WaitUntil(() => playerTurn.MyTurn != TurnState.None);
        _hitTextManager = playerTurn.MyTurn == TurnState.Player1 ? FindObjectOfType<Tab_HitText>().HitTexManagerForPlayer1 : FindObjectOfType<Tab_HitText>().HitTextManagerForPlayer2;
    }

    private void OnTakeDamage(BasePlayer basePlayer, int damage)
    {
        if (basePlayer != null)
        {
            HitTextManager.TextType textType = damage < 30 ? HitTextManager.TextType.Damage : HitTextManager.TextType.CriticalDamage;
            DisplayHitText(textType, GlobalFunctions.RedColorText((-damage).ToString()));
        }        
    }

    private void OnHitEnemy(int[] scores)
    {
        _playerHitsIndex = _playerTurnIndex + 1;

        int total = 0;

        GlobalFunctions.Loop<int>.Foreach(scores, value => { total += value; });
        Conditions<bool>.Compare(_playerHitsIndex < 3, () => OnSingleHit(total), ()=> OnBackToBackHit(total));
    }

    private void OnSingleHit(int total)
    {
        for (int i = 0; i < _soundController.SoundsList[1]._clips.Length; i++)
        {
            if (_soundController.SoundsList[1]._clips[i]._score >= total)
            {
                DisplayHitText(HitTextManager.TextType.Hit, "");
                break;
            }
        }
    }

    private void OnBackToBackHit(int total)
    {
        for (int i = 0; i < _soundController.SoundsList[2]._clips.Length; i++)
        {
            if (_soundController.SoundsList[2]._clips[i]._score >= _playerHitsIndex)
            {
                GetComboScore(total, i);
                DisplayHitText(HitTextManager.TextType.HitCombo, GlobalFunctions.BlueColorText("+" + (_playerHitsIndex - 2)));
                break;
            }
        }
    }

    private void DisplayHitText(HitTextManager.TextType textType, string text)
    {
        _hitTextManager.Display(textType, text);
    }

    private void GetComboScore(int total, int index)
    {
        float a = _soundController.SoundsList[2]._clips[index]._score;
        float b = a * 0.1f;
        float c = (total * b);

        _scoreController.GetScore(Mathf.RoundToInt(c), null);
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
