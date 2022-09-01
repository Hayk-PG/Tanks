using System;
using UnityEngine;
using UnityEngine.UI;

public partial class Tab_EndGame
{
    [Serializable] private struct UI
    {
        [SerializeField] internal Text _textXP;
        [SerializeField] internal Text _textPointsPlus;
        [SerializeField] internal Text _textLevel;
        [SerializeField] internal Slider _sliderXP;
        [SerializeField] internal CanvasGroup[] _canvasGroupGameResults;
        [SerializeField] internal Animator _textPointsPlusAnim;
    }
    [SerializeField] UI _ui;


    private void DisplayGameResult(GameResult gameResult)
    {
        GlobalFunctions.CanvasGroupActivity(_ui._canvasGroupGameResults[(int)gameResult], true);
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

    private void PointsPlus(int points)
    {
        _ui._textPointsPlusAnim.SetTrigger(Names.Play);
        SecondarySoundController.PlaySound(0, 3);

        if(_ui._textPointsPlusAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            _ui._textPointsPlus.text = "+" + points;
    }
}
