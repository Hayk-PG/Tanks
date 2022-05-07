using System.Collections;
using UnityEngine;

public partial class Tab_EndGame
{
    private float _currentVelocity;
    private bool _isCoroutineRunning = true;
    private IEnumerator Coroutine => DisplayController(_isCoroutineRunning);

    private void Start()
    {
        StartCoroutine(Coroutine);
    }

    private void SetImageTitleGlowColor(Color colorTitleGlow)
    {
        _ui._imageTitleGlow.color = colorTitleGlow;
    }

    private void SetTitleText(string textTitle)
    {
        _ui._textTitle.text = textTitle;
    }

    private void SetLevelText(int level)
    {
        _ui._textLevel.text = level.ToString();
    }

    private void SetSliderXPMinAndMaxValues(int min, int max)
    {
        _ui._sliderXP.minValue = min;
        _ui._sliderXP.maxValue = max;
    }

    private void SetSliderXPValue(float sliderXpValue)
    {
        _ui._sliderXP.value = sliderXpValue;
        _ui._textXP.text = _ui._sliderXP.value.ToString();
    }

    private IEnumerator DisplayController(bool _isCoroutineRunning)
    {
        SetLevelText(Data.Manager.Level);
        SetSliderXPMinAndMaxValues(Data.Manager.PointsSliderMinAndMaxValues[Data.Manager.Level, 0], Data.Manager.PointsSliderMinAndMaxValues[Data.Manager.Level, 1]);
        SetSliderXPValue(Data.Manager.Points);

        yield return new WaitForSeconds(1);
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, true);

        int currentLevel = Data.Manager.Level;

        int forPlayingPoints = 5000;
        int newPoints = 7500;
        int extraPoints = 2500;

        int step1 = (int)(_ui._sliderXP.value + forPlayingPoints);
        int step2 = step1 + newPoints;
        int step3 = step2 + extraPoints;

        while (_isCoroutineRunning)
        {
            print("coroutine is running");

            while(_ui._sliderXP.value < step1)
            {
                if(_ui._sliderXP.value < _ui._sliderXP.maxValue)
                { 
                    SetSliderXPValue(Mathf.SmoothDamp(_ui._sliderXP.value, step1, ref _currentVelocity, 1 * Time.deltaTime, 6500));
                    yield return null;
                }
                else
                {
                    LevelUpAndResetSlider(currentLevel);
                    yield return new WaitForSeconds(2);
                }
            }

            yield return new WaitUntil(() => _ui._sliderXP.value >= step1);
            yield return new WaitForSeconds(1);

            while (_ui._sliderXP.value < step2)
            {
                if (_ui._sliderXP.value < _ui._sliderXP.maxValue)
                {        
                    SetSliderXPValue(Mathf.SmoothDamp(_ui._sliderXP.value, step2, ref _currentVelocity, 1 * Time.deltaTime, 6500));
                    yield return null;
                }
                else
                {
                    LevelUpAndResetSlider(currentLevel);
                    yield return new WaitForSeconds(2);
                }
            }

            yield return new WaitUntil(() => _ui._sliderXP.value >= step2);
            yield return new WaitForSeconds(1);

            while (_ui._sliderXP.value < step3)
            {
                if (_ui._sliderXP.value < _ui._sliderXP.maxValue)
                {     
                    SetSliderXPValue(Mathf.SmoothDamp(_ui._sliderXP.value, step3, ref _currentVelocity, 1 * Time.deltaTime, 6500));
                    yield return null;
                }
                else
                {
                    LevelUpAndResetSlider(currentLevel);
                    yield return new WaitForSeconds(2);
                }
            }

            yield return new WaitUntil(() => _ui._sliderXP.value >= step3);

            _isCoroutineRunning = false;
            StopCoroutine(Coroutine);
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
