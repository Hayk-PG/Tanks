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
    private float _currentVelocity;
    private bool _isCoroutineRunning = true;

    public Action OnGameResultsFinished { get; set; }


    private IEnumerator DisplayController(bool _isCoroutineRunning, Values values)
    {
        SetLevelText(Data.Manager.Level);
        SetSliderXPMinAndMaxValues(Data.Manager.PointsSliderMinAndMaxValues[Data.Manager.Level, 0], Data.Manager.PointsSliderMinAndMaxValues[Data.Manager.Level, 1]);
        SetSliderXPValue(Data.Manager.Points);

        yield return new WaitForSeconds(1);
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, true);

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
            _isCoroutineRunning = false;
        }
    }

    private void LevelUpAndResetSlider(int currentLevel)
    {
        int newLevel = currentLevel + 1;
        SetLevelText(newLevel);
        SetSliderXPMinAndMaxValues(Data.Manager.PointsSliderMinAndMaxValues[newLevel, 0], Data.Manager.PointsSliderMinAndMaxValues[newLevel, 1]);
        SetSliderXPValue(Data.Manager.PointsSliderMinAndMaxValues[newLevel, 0]);
    }
}
