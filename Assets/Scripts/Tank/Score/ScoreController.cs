using ExitGames.Client.Photon;
using Photon.Pun;
using System;
using UnityEngine;

public class ScoreController : MonoBehaviour, IScore
{
    private TankController _tankController;

    private ScoreFromTerOccIndController _scoreFromTerOccIndController;

    private int _score;
    private int _scoreMultiplier = 1;

    public IDamage IDamage { get; set; }

    public PlayerTurn PlayerTurn { get; set; }

    public int Score
    {
        get => _score;
        set => _score = value;
    }
    public int MainScore { get; set; }

    public bool IsXpBoost { get; set; }

    public event Action<int, float> onDisplayPlayerScore;
    public Action<int> OnPlayerGetsPoints { get; set; }
    public Action<int[]> OnHitEnemy { get; set; }






    private void Awake()
    {       
        IDamage = Get<IDamage>.From(gameObject);

        PlayerTurn = Get<PlayerTurn>.From(gameObject);

        _tankController = Get<TankController>.From(gameObject);

        _scoreFromTerOccIndController = Get<ScoreFromTerOccIndController>.From(gameObject);

        Score = 10000;

        MainScore = Score;
    }

    private void OnEnable()
    {
        _tankController.OnInitialize += OnInitialize;

        if (MyPhotonNetwork.IsOfflineMode)
            GameSceneObjectsReferences.GameManagerBulletSerializer.OnTornado += OnTornado;
        else
            PhotonNetwork.NetworkingClient.EventReceived += OnTornado;
    }

    private void OnDisable()
    {
        _tankController.OnInitialize -= OnInitialize;

        GameSceneObjectsReferences.AmmoTabCustomization.OnPlayerWeaponChanged -= OnPlayerWeaponChanged;

        if (_scoreFromTerOccIndController != null)
            _scoreFromTerOccIndController.OnGetScoreFromTerOccInd -= OnGetScoreFromTerOccInd;

        if (MyPhotonNetwork.IsOfflineMode)
            GameSceneObjectsReferences.GameManagerBulletSerializer.OnTornado -= OnTornado;
        else
            PhotonNetwork.NetworkingClient.EventReceived -= OnTornado;
    }

    private void OnInitialize()
    {
        GameSceneObjectsReferences.AmmoTabCustomization.OnPlayerWeaponChanged += OnPlayerWeaponChanged;

        if (_scoreFromTerOccIndController != null)
            _scoreFromTerOccIndController.OnGetScoreFromTerOccInd += OnGetScoreFromTerOccInd;
    }

    public void BoostXp(bool isXpBoost) => IsXpBoost = isXpBoost;

    private void ReceiveTornadoScore(object[] data)
    {
        if ((string)data[2] == name && (string)data[0] != name)
            GetScore(150, null);
    }

    private void OnTornado(object[] data) => ReceiveTornadoScore(data);

    private void OnTornado(EventData data)
    {
        if (data.Code == EventInfo.Code_TornadoDamage)
            ReceiveTornadoScore((object[])data.CustomData);
    }

    private void OnPlayerWeaponChanged(AmmoTypeButton ammoTypeButton)
    {
        if (!ammoTypeButton._properties.IsUnlocked)
            UpdateScore(-ammoTypeButton._properties.Price, 0);    
    }

    private void OnSupportOrPropsChanged(AmmoTypeButton supportOrPropsTypeButton) => UpdateScore(-supportOrPropsTypeButton._properties.Price, 0);

    public void GetScore(int score, IDamage iDamage)
    {
        Conditions<bool>.Compare(iDamage != IDamage || iDamage == null, () => UpdateScore(score, 0.5f), null);
    }

    private void UpdateScore(int score, float waitForSeconds)
    {
        if (score == 0)
            return;

        int sc = IsXpBoost && score > 0 ? score * 2 : score;
        int multipliedScore = sc > 0 ? sc * _scoreMultiplier : sc;

        Score += multipliedScore;

        if (multipliedScore > 0)
            MainScore += multipliedScore;

        onDisplayPlayerScore?.Invoke(multipliedScore, waitForSeconds);

        OnPlayerGetsPoints?.Invoke(Score);
    }

    public void HitEnemyAndGetScore(int[] scores, IDamage enemyDamage)
    {
        if (enemyDamage != IDamage)
        {
            PlayHitSoundEffect();

            AnnouncePlayerHitFeedback();

            OnHitEnemy?.Invoke(scores);
        }
    }

    private void PlayHitSoundEffect()
    {
        if (_tankController.BasePlayer == null)
            return;

        SecondarySoundController.PlaySound(0, 1);
    }

    private void AnnouncePlayerHitFeedback()
    {
        if (_tankController.BasePlayer == null)
            return;

        GameSceneObjectsReferences.PlayerFeedbackAnnouncer.AnnounceFeedback(6, UnityEngine.Random.Range(0, SoundController.Instance.SoundsList[6]._clips.Length));
    }

    private void OnGetScoreFromTerOccInd() => UpdateScore(100, 0.5f);

    public void SetScoreMultiplier(int value) => _scoreMultiplier = value;

    public void GetScoreFromDropBoxPanel(int multiplier)
    {
        if (_tankController.BasePlayer != null)
        {
            int m = multiplier <= 2 ? 1 : multiplier - 1;

            int score = Score <= 100 ? 100 * m : Score * m;

            GetScore(score, null);
        }
    }
}
