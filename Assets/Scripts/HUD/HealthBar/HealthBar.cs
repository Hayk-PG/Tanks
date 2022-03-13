using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Serializable]
    private struct Sliders
    {
        [SerializeField]
        internal Slider _slider;

        [SerializeField]
        internal LastHealthFill _lastHealthFill;

        [SerializeField]
        internal Text _hpText;
    }

    [SerializeField]
    private Sliders[] _sliders;


    public void OnUpdateHealthBar(TurnState turnState, int newValue)
    {
        switch (turnState)
        {
            case TurnState.Player1: OnHealthBar(0, newValue); break;
            case TurnState.Player2: OnHealthBar(1, newValue); break;
        }
    }

    private void OnHealthBar(int index, int newValue)
    {
        _sliders[index]._slider.value = newValue;
        _sliders[index]._hpText.text = newValue + "/" + "100";
        _sliders[index]._lastHealthFill.OnUpdate(_sliders[index]._slider.value / 100);       
    }
}
