using System;
using System.Collections;
using UnityEngine;

public partial class Tab_EndGame
{
    private struct Values
    {
        internal int _currentLevel;
        internal int _playPoints;       
        internal int _experiencePoints;
        internal int _winPoints;
        internal int _playerBeforeGamePoints;
        internal int _playerNewGainedPoints;

        internal int _step1;
        internal int _step2;
        internal int _step3;

        internal Values(int currentLevel, int playPoints, int experiencePoints, int winPoints,  int playerBeforeGamePoints, int playerNewGainedPoints)
        {
            _currentLevel = currentLevel;
            _playPoints = playPoints;
            _experiencePoints = experiencePoints;
            _winPoints = winPoints;
            _playerBeforeGamePoints = playerBeforeGamePoints;
            _playerNewGainedPoints = playerNewGainedPoints;

            _step1 = _playerBeforeGamePoints + _playPoints;
            _step2 = _step1 + _experiencePoints;
            _step3 = _step2 + _playerNewGainedPoints + _winPoints;
        }
    }
    public struct Result
    {
        public int _win;
        public int _lose;
        public int _points;
        public int _level;
    }
    private float _currentVelocity;
    private bool _isCoroutineRunning = true;

    public Action OnGameResultsFinished { get; set; }
    public event Action<Result> onResult;



    private void DeactivateTanks()
    {
        GlobalFunctions.Loop<TankController>.Foreach(FindObjectsOfType<TankController>(), tank => { tank.gameObject.SetActive(false); });
    }

    private IEnumerator DisplayController(bool _isCoroutineRunning, Values values, GameResult gameResult)
    {
        int level = Data.Manager.Statistics[Keys.Level];
        int sliderMin = Data.Manager.PointsSliderMinAndMaxValues[level, 0];
        int sliderMax = Data.Manager.PointsSliderMinAndMaxValues[level, 1];
        int currentPoints = Data.Manager.Statistics[Keys.Points];

        SetLevelText(level);
        SetSliderXPMinAndMaxValues(sliderMin, sliderMax);
        SetSliderXPValue(currentPoints);
        DeactivateTanks();

        yield return new WaitForSeconds(1);
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, true);

        int clipIndex = gameResult == GameResult.Win ? Indexes.Combat_Announcer_Male_Effect_You_Win_1 : Indexes.Combat_Announcer_Male_Effect_You_Lose;
        SoundController.MusicSRCVolume(SoundController.MusicVolume.Down);
        SoundController.PlaySound(0, clipIndex, out float clipLength);
        yield return new WaitForSeconds(clipLength);
        SoundController.MusicSRCVolume(SoundController.MusicVolume.Up);

        while (_isCoroutineRunning)
        {
            print("coroutine is running");
            PointsPlus(values._playPoints);

            while(_ui._sliderXP.value < values._step1)
            {
                if(_ui._sliderXP.value < _ui._sliderXP.maxValue)
                {
                    SetSliderXPValue(Mathf.SmoothDamp(_ui._sliderXP.value, values._step1, ref _currentVelocity, 1 * Time.deltaTime, 6500));
                    yield return null;
                }
                else
                {
                    LevelUpAndResetSlider(values._currentLevel);
                    yield return new WaitForSeconds(2);
                }
            }

            yield return new WaitUntil(() => _ui._sliderXP.value >= values._step1);
            yield return new WaitForSeconds(1);

            PointsPlus(values._experiencePoints);

            while (_ui._sliderXP.value < values._step2)
            {
                if (_ui._sliderXP.value < _ui._sliderXP.maxValue)
                {        
                    SetSliderXPValue(Mathf.SmoothDamp(_ui._sliderXP.value, values._step2, ref _currentVelocity, 1 * Time.deltaTime, 6500));
                    yield return null;
                }
                else
                {
                    LevelUpAndResetSlider(values._currentLevel);
                    yield return new WaitForSeconds(2);
                }
            }

            yield return new WaitUntil(() => _ui._sliderXP.value >= values._step2);
            yield return new WaitForSeconds(1);

            PointsPlus(values._playerNewGainedPoints);

            while (_ui._sliderXP.value < values._step3)
            {
                if (_ui._sliderXP.value < _ui._sliderXP.maxValue)
                {     
                    SetSliderXPValue(Mathf.SmoothDamp(_ui._sliderXP.value, values._step3, ref _currentVelocity, 1 * Time.deltaTime, 6500));
                    yield return null;
                }
                else
                {
                    LevelUpAndResetSlider(values._currentLevel);
                    yield return new WaitForSeconds(2);
                }
            }

            yield return new WaitUntil(() => _ui._sliderXP.value >= values._step3);
            OnGameResultsFinished?.Invoke();
            CalculateResult(gameResult, (int)_ui._sliderXP.value, int.Parse(_ui._textLevel.text));
            _isCoroutineRunning = false;
        }
    }

    private void LevelUpAndResetSlider(int currentLevel)
    {
        int newLevel = currentLevel + 1;
        SetLevelText(newLevel);
        SetSliderXPMinAndMaxValues(Data.Manager.PointsSliderMinAndMaxValues[newLevel, 0], Data.Manager.PointsSliderMinAndMaxValues[newLevel, 1]);
        SetSliderXPValue(Data.Manager.PointsSliderMinAndMaxValues[newLevel, 0]);
        SecondarySoundController.PlaySound(0, 4);
    }

    private void CalculateResult(GameResult gameResult, int points, int level)
    {
        Result result = new Result
        {
            _win = gameResult == GameResult.Win ? 1 : 0,
            _lose = gameResult == GameResult.Win ? 0 : 1,
            _points = points,
            _level = level
        };

        onResult?.Invoke(result);
    }
}
