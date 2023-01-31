using ExitGames.Client.Photon;
using Photon.Pun;
using System;
using UnityEngine;

public class ScoreController : MonoBehaviour, IScore
{
    public IDamage IDamage { get; set; }
    private TankController _tankController;
    private AmmoTabCustomization _ammoTabCustomization;
    private PropsTabCustomization _propsTabCustomization;
    private SupportsTabCustomization _supportTabCustomization;
    private GetScoreFromTerOccInd _getScoreFromTerOccInd;
    private GameManagerBulletSerializer _gameManagerBulletSerializer;

    private int _score;

    public PlayerTurn PlayerTurn { get; set; }
    public int Score
    {
        get => _score;
        set => _score = value;
    }
    public int MainScore { get; set; }
    public bool IsXpBoost { get; set; }

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
        _gameManagerBulletSerializer = FindObjectOfType<GameManagerBulletSerializer>();

        Score = 0;
        MainScore = 0;
    }

    private void OnEnable()
    {
        _tankController.OnInitialize += OnInitialize;

        if (MyPhotonNetwork.IsOfflineMode)
            _gameManagerBulletSerializer.OnTornado += OnTornado;
        else
            PhotonNetwork.NetworkingClient.EventReceived += OnTornado;
    }

    private void OnDisable()
    {
        _tankController.OnInitialize -= OnInitialize;
        _ammoTabCustomization.OnPlayerWeaponChanged -= OnPlayerWeaponChanged;
        _propsTabCustomization.OnSupportOrPropsChanged -= OnSupportOrPropsChanged;
        _supportTabCustomization.OnSupportOrPropsChanged -= OnSupportOrPropsChanged;

        if (_getScoreFromTerOccInd != null)
            _getScoreFromTerOccInd.OnGetScoreFromTerOccInd -= OnGetScoreFromTerOccInd;

        if (MyPhotonNetwork.IsOfflineMode)
            _gameManagerBulletSerializer.OnTornado -= OnTornado;
        else
            PhotonNetwork.NetworkingClient.EventReceived -= OnTornado;
    }

    private void OnInitialize()
    {
        _ammoTabCustomization.OnPlayerWeaponChanged += OnPlayerWeaponChanged;
        _propsTabCustomization.OnSupportOrPropsChanged += OnSupportOrPropsChanged;
        _supportTabCustomization.OnSupportOrPropsChanged += OnSupportOrPropsChanged;

        if (_getScoreFromTerOccInd != null)
            _getScoreFromTerOccInd.OnGetScoreFromTerOccInd += OnGetScoreFromTerOccInd;
    }

    public void BoostXp(bool isXpBoost) => IsXpBoost = isXpBoost;

    private void ReceiveTornadoScore(object[] data)
    {
        if((string)data[2] == name && (string)data[0] != name)
        {
            GetScore(150, null);
        }
    }

    private void OnTornado(object[] data)
    {
        ReceiveTornadoScore(data);
    }

    private void OnTornado(EventData data)
    {
        if(data.Code == EventInfo.Code_TornadoDamage)
        {
            ReceiveTornadoScore((object[])data.CustomData);
        }
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
        int sc = IsXpBoost && score > 0 ? score * 2 : score;

        Score += sc;

        if (sc > 0)
            MainScore += sc;

        OnDisplayTempPoints?.Invoke(sc, waitForSeconds);
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
