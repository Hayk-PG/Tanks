using UnityEngine;
using UnityEngine.UI;

public class EndGameUIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup[] _gameResultPanelsCanvasGroups;
    [SerializeField] private Text _txtCurrentXP;
    [SerializeField] private Text _txtReceivedXP;
    [SerializeField] private Text _txtLevel;
    [SerializeField] private Slider _sldrXP;
    [SerializeField] private Animator _animReceivedXP;

    public int SliderMin { get => (int)_sldrXP.minValue; private set => _sldrXP.minValue = value; }
    public int SliderMax { get => (int)_sldrXP.maxValue; private set => _sldrXP.maxValue = value; }
    public int SliderValue { get => Mathf.RoundToInt(_sldrXP.value); private set => _sldrXP.value = value; }
    public int CurrentXP { get => int.Parse(_txtCurrentXP.text); private set => _txtCurrentXP.text = value.ToString(); }
    public int Level { get => int.Parse(_txtLevel.text); private set => _txtLevel.text = value.ToString(); }
   


    private void Start()
    {
        _txtCurrentXP.text = "0";
        _txtReceivedXP.text = "0";
        _txtLevel.text = "0";
        _sldrXP.value = 0;
    }

    public void SetGameResultPanelVisible(int index) => GlobalFunctions.CanvasGroupActivity(_gameResultPanelsCanvasGroups[index], true);

    public void SetSliderLimits(int min, int max)
    {
        SliderMin = min;
        SliderMax = max;
    }

    private int SliderRate(int value)
    {
        return value <= 150 ? 1 : value <= 300 ? 2 : value <= 500 ? 3 : value <= 1000 ? 10 : value <= 2500 ? 20 : value <= 5000 ? 50 : 100;
    }

    public void SetSliderValueAndCurrentXP(int value)
    {
        if (SliderValue < value)
            SliderValue += SliderRate(value);
        else
            SliderValue = value;

        CurrentXP = SliderValue;
    }

    public void SetLevel(int level)
    {
        Level = level;
    }

    public void SetReceivedXP(int points)
    {
        _animReceivedXP.SetTrigger(Names.Play);

        SecondarySoundController.PlaySound(0, 3);

        if (_animReceivedXP.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            _txtReceivedXP.text = "+" + points;
    } 
}
