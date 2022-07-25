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
    [Serializable] [SerializeField] private struct RequiredItems
    {
        [SerializeField] internal RequiredItemsBar[] _requiredItemsBar;
    }
    [Serializable] [SerializeField] private struct Buttons
    {
        [SerializeField] internal Button _buttonGetItNow;
        [SerializeField] internal Button _buttonBuild;
        [SerializeField] internal Button _buttonSelect;
        [SerializeField] internal Text _textPrice;
        [SerializeField] internal Text _textBuildTime;
    }
    [Serializable] [SerializeField] private struct CanvasGroups
    {
        [SerializeField] internal CanvasGroup[] _part1;
        [SerializeField] internal CanvasGroup[] _part2;
        [SerializeField] internal CanvasGroup[] _part3;
    }

    [SerializeField] private TankStats _tankStats;
    [SerializeField] private RequiredItems _requiredItems;
    [SerializeField] private Buttons _buttons;
    [SerializeField] private CanvasGroups _canvasGroups;

    public struct Info
    {
        public int _tankSpeedValue;
        public int _tankArmorValue;
        public int _getItNowPrice;
        public string _buildTime;

        public Info(int tankSpeedValue, int tankArmorValue, int getItNowPrice, string buildTime)
        {
            _tankSpeedValue = tankSpeedValue;
            _tankArmorValue = tankArmorValue;
            _getItNowPrice = getItNowPrice;
            _buildTime = buildTime;
        }
    }

    public struct RequiredItemsInfo
    {
        public TankInfo.Items[] _items;

        public RequiredItemsInfo(TankInfo.Items[] items)
        {
            _items = items;
        }
    }

    private void Stats(Info info)
    {
        _tankStats._textTankSpeedValue.text = info._tankSpeedValue.ToString();
        _tankStats._textTankArmorValue.text = info._tankArmorValue.ToString();
    }

    private void ButtonsText(Info info)
    {
        _buttons._textPrice.text = info._getItNowPrice.ToString();
        _buttons._textBuildTime.text = info._buildTime;
    }

    public void DisplatRequiredItems(RequiredItemsInfo requiredItemsInfo)
    {
        GlobalFunctions.Loop<RequiredItemsBar>.Foreach(_requiredItems._requiredItemsBar, item => { item.gameObject.SetActive(false); });

        if (requiredItemsInfo._items.Length > 0)
        {
            for (int i = 0; i < requiredItemsInfo._items.Length; i++)
            {
                _requiredItems._requiredItemsBar[i].gameObject.SetActive(true);
                _requiredItems._requiredItemsBar[i].Icon = requiredItemsInfo._items[i]._item.Icon;
                _requiredItems._requiredItemsBar[i].Info = requiredItemsInfo._items[i]._number.ToString();
                _requiredItems._requiredItemsBar[i].IsChecked = Data.Manager.ItemCountByItemType(requiredItemsInfo._items[i]._item.Type) >= requiredItemsInfo._items[i]._number;
            }
        }
    }

    public void TankNotOwnedScreen(Info info)
    {
        GlobalFunctions.Loop<CanvasGroup>.Foreach(_canvasGroups._part1, canvasGroup => { GlobalFunctions.CanvasGroupActivity(canvasGroup, true); });
        GlobalFunctions.Loop<CanvasGroup>.Foreach(_canvasGroups._part2, canvasGroup => { GlobalFunctions.CanvasGroupActivity(canvasGroup, true); });
        GlobalFunctions.Loop<CanvasGroup>.Foreach(_canvasGroups._part3, canvasGroup => { GlobalFunctions.CanvasGroupActivity(canvasGroup, false); });
        Stats(info);
        ButtonsText(info);
    }

    public void TankOwnedScreen(Info info)
    {
        GlobalFunctions.Loop<CanvasGroup>.Foreach(_canvasGroups._part1, canvasGroup => { GlobalFunctions.CanvasGroupActivity(canvasGroup, false); });
        GlobalFunctions.Loop<CanvasGroup>.Foreach(_canvasGroups._part2, canvasGroup => { GlobalFunctions.CanvasGroupActivity(canvasGroup, true); });
        GlobalFunctions.Loop<CanvasGroup>.Foreach(_canvasGroups._part3, canvasGroup => { GlobalFunctions.CanvasGroupActivity(canvasGroup, true); });
        Stats(info);
    }

    public void ResetTanksInfoScreen()
    {
        GlobalFunctions.Loop<CanvasGroup>.Foreach(_canvasGroups._part1, canvasGroup => { GlobalFunctions.CanvasGroupActivity(canvasGroup, false); });
        GlobalFunctions.Loop<CanvasGroup>.Foreach(_canvasGroups._part2, canvasGroup => { GlobalFunctions.CanvasGroupActivity(canvasGroup, false); });
        GlobalFunctions.Loop<CanvasGroup>.Foreach(_canvasGroups._part2, canvasGroup => { GlobalFunctions.CanvasGroupActivity(canvasGroup, false); });
    }
}
