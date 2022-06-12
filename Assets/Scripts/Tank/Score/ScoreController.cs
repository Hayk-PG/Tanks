using System;
using System.Linq;
using UnityEngine;

public class ScoreController : MonoBehaviour, IScore
{
    public PlayerTurn PlayerTurn { get; set; }
    public IDamage IDamage { get; set; }
    private AmmoTabCustomization _ammoTabCustomization;
    private GetScoreFromTerOccInd _getScoreFromTerOccInd;

    [SerializeField]
    private int _score;

    public int Score
    {
        get => _score;
        set => _score = value;
    }
    public Action<int, float> OnDisplayTempPoints { get; set; }
    public Action<int> OnPlayerGetsPoints { get; set; }
    public Action<int> OnHitEnemy { get; set; }



    private void Awake()
    {       
        IDamage = Get<IDamage>.From(gameObject);
        PlayerTurn = Get<PlayerTurn>.From(gameObject);
        _ammoTabCustomization = FindObjectOfType<AmmoTabCustomization>();
        _getScoreFromTerOccInd = GetComponent<GetScoreFromTerOccInd>();

        Score = 0;
    }

    private void OnEnable()
    {
        _ammoTabCustomization.OnPlayerWeaponChanged += OnPlayerWeaponChanged;
        if (_getScoreFromTerOccInd != null) _getScoreFromTerOccInd.OnGetScoreFromTerOccInd += OnGetScoreFromTerOccInd;
    }

    private void OnDisable()
    {
        _ammoTabCustomization.OnPlayerWeaponChanged -= OnPlayerWeaponChanged;
        if (_getScoreFromTerOccInd != null) _getScoreFromTerOccInd.OnGetScoreFromTerOccInd -= OnGetScoreFromTerOccInd;
    }

    private void OnPlayerWeaponChanged(AmmoTypeButton ammoTypeButton)
    {
        if (!ammoTypeButton._properties.IsUnlocked)
        {
            Score -= ammoTypeButton._properties.UnlockPoints;
            OnDisplayTempPoints?.Invoke(-ammoTypeButton._properties.UnlockPoints, 0);
        }
    }

    public void GetScore(int score, IDamage iDamage)
    {
        Conditions<bool>.Compare(iDamage != IDamage || iDamage == null, () => UpdateScore(score), null);
    }

    private void UpdateScore(int score)
    {
        Score += score;
        OnDisplayTempPoints?.Invoke(score, 0.5f);
        OnPlayerGetsPoints?.Invoke(Score);
    }

    public void HitEnemyAndGetScore(int score, IDamage enemyDamage)
    {
        if (enemyDamage != IDamage) OnHitEnemy?.Invoke(score);
    }

    private void OnGetScoreFromTerOccInd()
    {
        UpdateScore(100);
    }
}
