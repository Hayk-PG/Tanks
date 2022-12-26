using System;
using System.Collections;
using UnityEngine;

public class GameResultProcessor : MonoBehaviour
{
    private WinnerLoserIdentifier _winnerLoserIdentifier;
    private EndGameUIManager _endGameUIManager;

    public event Action onBeginResultProcess;


    private void Awake()
    {
        _winnerLoserIdentifier = Get<WinnerLoserIdentifier>.From(gameObject);
        _endGameUIManager = Get<EndGameUIManager>.From(gameObject);
    }

    private void OnEnable() => _winnerLoserIdentifier.onIdentified += ProcessLocalPlayerGameResult;

    private void OnDisable() => _winnerLoserIdentifier.onIdentified -= ProcessLocalPlayerGameResult;

    private void ProcessLocalPlayerGameResult(ScoreController scoreController, bool isWin)
    {
        StartCoroutine(ProcessLocalPlayerGameResultCoroutine(scoreController, isWin));
    }

    private void InitializeEndGameUIManagerValues(bool isWin)
    {
        int level = Data.Manager.Statistics[Keys.Level];

        _endGameUIManager.SetGameResultPanelVisible(isWin ? 0 : 1);
        _endGameUIManager.SetLevel(level);
        _endGameUIManager.SetSliderLimits(Data.Manager.PointsSliderMinAndMaxValues[level, 0], Data.Manager.PointsSliderMinAndMaxValues[level, 1]);
        _endGameUIManager.SetSliderValueAndCurrentXP(Data.Manager.Statistics[Keys.Points]);
    }

    private GameResultValues GameResultValues(ScoreController scoreController, bool isWin)
    {
        int lastSavedPoints = Data.Manager.Statistics[Keys.Points];
        int newPoints = scoreController.MainScore;
        int playPoints = 50;
        int _experiencePoints = 150;
        int winPoints = isWin ? 300 : 0;
        int stageFirst = lastSavedPoints + playPoints;
        int stageSecond = stageFirst + _experiencePoints;
        int stageThird = stageSecond + newPoints + winPoints;

        return new GameResultValues
        {
            _lastSavedPoints = lastSavedPoints,
            _newPoints = newPoints,
            _playPoints = playPoints,
            _experiencePoints = _experiencePoints,
            _winPoints = winPoints,
            _stageFirst = stageFirst,
            _stageSecond = stageSecond,
            _stageThird = stageThird
        };
    }

    private void DeactivateTanks() => GlobalFunctions.Loop<TankController>.Foreach(FindObjectsOfType<TankController>(), tank => { tank.gameObject.SetActive(false); });

    private IEnumerator DelayCalculations()
    {
        yield return new WaitForSeconds(1);
        onBeginResultProcess?.Invoke();
    }

    private IEnumerator AnnounceGameResult(bool isWin)
    {
        yield return new WaitForSeconds(1);

        int clipIndex = isWin ? Indexes.Combat_Announcer_Male_Effect_You_Win_1 : Indexes.Combat_Announcer_Male_Effect_You_Lose;
        SoundController.MusicSRCVolume(SoundController.MusicVolume.Down);
        SoundController.PlaySound(0, clipIndex, out float clipLength);

        yield return new WaitForSeconds(clipLength);

        SoundController.MusicSRCVolume(SoundController.MusicVolume.Up);
    }

    private void LevelUpAndResetSlider()
    {
        int newLevel = _endGameUIManager.Level + 1;
        _endGameUIManager.SetLevel(newLevel);
        _endGameUIManager.SetSliderLimits(Data.Manager.PointsSliderMinAndMaxValues[newLevel, 0], Data.Manager.PointsSliderMinAndMaxValues[newLevel, 1]);
        _endGameUIManager.SetSliderValueAndCurrentXP(Data.Manager.PointsSliderMinAndMaxValues[newLevel, 0]);
        SecondarySoundController.PlaySound(0, 4);
    }

    private IEnumerator CalculateScores(int xp, int stage)
    {
        _endGameUIManager.SetReceivedXP(xp);

        int sliderValue = _endGameUIManager.SliderValue;
        int sliderMax = _endGameUIManager.SliderMax;

        while (sliderValue <= stage)
        {
            if (sliderValue < sliderMax)
            {
                _endGameUIManager.SetSliderValueAndCurrentXP(stage);
                yield return null;
            }
            else
            {
                LevelUpAndResetSlider();
                yield return new WaitForSeconds(2);
            }
        }

        yield return new WaitUntil(() => _endGameUIManager.SliderValue >= stage);
        yield return new WaitForSeconds(1);
    }

    private IEnumerator ProcessLocalPlayerGameResultCoroutine(ScoreController scoreController, bool isWin)
    {
        InitializeEndGameUIManagerValues(isWin);
        GameResultValues gameResultValues = GameResultValues(scoreController, isWin);
        DeactivateTanks();
        StartCoroutine(DelayCalculations());
        StartCoroutine(AnnounceGameResult(isWin));

        while (true)
        {
            StartCoroutine(CalculateScores(gameResultValues._playPoints, gameResultValues._stageFirst));
            StartCoroutine(CalculateScores(gameResultValues._experiencePoints, gameResultValues._stageSecond));
            StartCoroutine(CalculateScores(gameResultValues._newPoints, gameResultValues._stageThird));
        }
    }
}
