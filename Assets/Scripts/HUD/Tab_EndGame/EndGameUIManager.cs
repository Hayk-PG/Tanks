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
    public int SliderValue { get => (int)_sldrXP.value; private set => _sldrXP.value = value; }
    public int CurrentXP { get => int.Parse(_txtCurrentXP.text); private set => _txtCurrentXP.text = value.ToString(); }
    public int Level { get => int.Parse(_txtLevel.text); private set => _txtLevel.text = value.ToString(); }
   


    private void Awake()
    {
        _txtCurrentXP.text = "0";
        _txtReceivedXP.text = "0";
        _txtLevel.text = "0";
        _sldrXP.value = 0;
    }

    public void SetGameResultPanelVisible(int index)
    {
        GlobalFunctions.CanvasGroupActivity(_gameResultPanelsCanvasGroups[index], true);
    }

    public void SetSliderLimits(int min, int max)
    {
        SliderMin = min;
        SliderMax = max;
    }

    public void SetSliderValueAndCurrentXP(int value)
    {
        SliderValue = (int)Mathf.MoveTowards(_sldrXP.value, value, 5 * Time.deltaTime);
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
