using System;
using UnityEngine;

public class ScoreController : MonoBehaviour, IScore
{
    public PlayerTurn PlayerTurn { get; set; }
    public IDamage IDamage { get; set; }
    private TankController _tankController;
    private AmmoTabCustomization _ammoTabCustomization;
    private PropsTabCustomization _propsTabCustomization;
    private SupportsTabCustomization _supportTabCustomization;
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
    public Action<int[]> OnHitEnemy { get; set; }



    private void Awake()
    {       
        IDamage = Get<IDamage>.From(gameObject);
        PlayerTurn = Get<PlayerTurn>.From(gameObject);
        _tankController = Get<TankController>.From(gameObject);
        _ammoTabCustomization = FindObjectOfType<AmmoTabCustomization>();
        _propsTabCustomization = FindObjectOfType<PropsTabCustomization>();
        _supportTabCustomization = FindObjectOfType<SupportsTabCustomization>();
        _getScoreFromTerOccInd = GetComponent<GetScoreFromTerOccInd>();

        Score = 0;
    }

    private void OnEnable()
    {
        _tankController.OnInitialize += OnInitialize;
    }

    private void OnDisable()
    {
        _tankController.OnInitialize -= OnInitialize;
        _ammoTabCustomization.OnPlayerWeaponChanged -= OnPlayerWeaponChanged;
        _propsTabCustomization.OnSupportOrPropsChanged -= OnSupportOrPropsChanged;
        _supportTabCustomization.OnSupportOrPropsChanged -= OnSupportOrPropsChanged;

        if (_getScoreFromTerOccInd != null)
            _getScoreFromTerOccInd.OnGetScoreFromTerOccInd -= OnGetScoreFromTerOccInd;
    }

    private void OnInitialize()
    {
        _ammoTabCustomization.OnPlayerWeaponChanged += OnPlayerWeaponChanged;
        _propsTabCustomization.OnSupportOrPropsChanged += OnSupportOrPropsChanged;
        _supportTabCustomization.OnSupportOrPropsChanged += OnSupportOrPropsChanged;

        if (_getScoreFromTerOccInd != null)
            _getScoreFromTerOccInd.OnGetScoreFromTerOccInd += OnGetScoreFromTerOccInd;
    }

    private void OnPlayerWeaponChanged(AmmoTypeButton ammoTypeButton)
    {
        if (!ammoTypeButton._properties.IsUnlocked)
            UpdateScore(-ammoTypeButton._properties.RequiredScoreAmmount, 0);    

        if (ammoTypeButton._properties.Minutes > 0 || ammoTypeButton._properties.Seconds > 0)
            ammoTypeButton.StartTimerCoroutine();
    }

    private void OnSupportOrPropsChanged(AmmoTypeButton supportOrPropsTypeButton)
    {
        UpdateScore(-supportOrPropsTypeButton._properties.RequiredScoreAmmount, 0);
        supportOrPropsTypeButton.StartTimerCoroutine();
    }

    public void GetScore(int score, IDamage iDamage)
    {
        Conditions<bool>.Compare(iDamage != IDamage || iDamage == null, () => UpdateScore(score, 0.5f), null);
    }

    private void UpdateScore(int score, float waitForSeconds)
    {
        Score += score;
        OnDisplayTempPoints?.Invoke(score, waitForSeconds);
        OnPlayerGetsPoints?.Invoke(Score);
    }

    public void HitEnemyAndGetScore(int[] scores, IDamage enemyDamage)
    {
        if (enemyDamage != IDamage)
        {
            OnHitEnemy?.Invoke(scores);
        }
    }

    private void OnGetScoreFromTerOccInd()
    {
        UpdateScore(100, 0.5f);
    }

    public void GetScoreFromWoodBox(out bool isDone, out string text)
    {
        isDone = false;
        text = "";

        if (_tankController.BasePlayer != null)
        {
            GetScore(500, null);
            isDone = true;
            text = "+" + 500;
        }
    }
}
