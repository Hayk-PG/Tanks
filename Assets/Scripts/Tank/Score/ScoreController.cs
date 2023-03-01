using ExitGames.Client.Photon;
using Photon.Pun;
using System;
using UnityEngine;

public class ScoreController : MonoBehaviour, IScore
{
    private TankController _tankController;

    private ScoreFromTerOccIndController _scoreFromTerOccIndController;

    private int _score;
    
    public IDamage IDamage { get; set; }
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

        _scoreFromTerOccIndController = Get<ScoreFromTerOccIndController>.From(gameObject);

        Score = 0;

        MainScore = Score;
    }

    private void OnEnable()
    {
        _tankController.OnInitialize += OnInitialize;

        if (MyPhotonNetwork.IsOfflineMode)
            GameSceneObjectsReferences.GameManagerBulletSerializer.OnTornado += OnTornado;
        else
            PhotonNetwork.NetworkingClient.EventReceived += OnTornado;

        GameSceneObjectsReferences.DropBoxSelectionPanelDoubleXp.onDoubleXp += GetScoreFromDropBoxPanel;

        GameSceneObjectsReferences.DropBoxSelectionPanelScores.onScores += (scores) => { GetScore(scores, null); };
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

        GameSceneObjectsReferences.DropBoxSelectionPanelDoubleXp.onDoubleXp -= GetScoreFromDropBoxPanel;

        GameSceneObjectsReferences.DropBoxSelectionPanelScores.onScores -= (scores) => { GetScore(scores, null); };
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

    private void OnTornado(object[] data)
    {
        ReceiveTornadoScore(data);
    }

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

    private void OnSupportOrPropsChanged(AmmoTypeButton supportOrPropsTypeButton)
    {
        UpdateScore(-supportOrPropsTypeButton._properties.Price, 0);
    }

    public void GetScore(int score, IDamage iDamage)
    {
        Conditions<bool>.Compare(iDamage != IDamage || iDamage == null, () => UpdateScore(score, 0.5f), null);
    }

    private void UpdateScore(int score, float waitForSeconds)
    {
        if (score == 0)
            return;

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

    private void OnGetScoreFromTerOccInd() => UpdateScore(100, 0.5f);

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
