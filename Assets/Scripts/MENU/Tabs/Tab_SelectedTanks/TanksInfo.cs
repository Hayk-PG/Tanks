using System;
using UnityEngine;
using UnityEngine.UI;

public class TanksInfo : MonoBehaviour
{
    [Serializable] [SerializeField] private struct TankStats
    {
        [SerializeField] internal Text _textTankSpeedValue;
        [SerializeField] internal Text _textTankRangeValue;
        [SerializeField] internal Text _textTankArmorValue;
    }
    [Serializable] [SerializeField] private struct Buttons
    {
        [SerializeField] internal Button _buttonGetItNow;
        [SerializeField] internal Button _buttonBuild;
        [SerializeField] internal Button _buttonSelect;
        [SerializeField] internal Text _textPrice;
        [SerializeField] internal Text _textBuildTime;
    }

    [SerializeField] private TankStats _tankStats;
    [SerializeField] private Buttons _buttons;


    public struct Info
    {
        public int _tankSpeedValue;
        public int _tankRangeValue;
        public int _tankArmorValue;
        public int _getItNowPrice;
        public string _buildTime;

        public Info(int tankSpeedValue, int tankRangeValue, int tankArmorValue, int getItNowPrice, string buildTime)
        {
            _tankSpeedValue = tankSpeedValue;
            _tankRangeValue = tankRangeValue;
            _tankArmorValue = tankArmorValue;
            _getItNowPrice = getItNowPrice;
            _buildTime = buildTime;
        }
    }

    public void Display(Info info)
    {
        _tankStats._textTankSpeedValue.text = info._tankSpeedValue.ToString();
        _tankStats._textTankRangeValue.text = info._tankRangeValue.ToString();
        _tankStats._textTankArmorValue.text = info._tankArmorValue.ToString();

        _buttons._textPrice.text = info._getItNowPrice.ToString();
        _buttons._textBuildTime.text = info._buildTime;
    }
}
