using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SubTabUnlock : MonoBehaviour
{
    [SerializeField] [Space]
    private CanvasGroup _canvasGroup, _canvasGroupSubTabUnlock;

    [SerializeField] [Space]
    private Image _img;

    [SerializeField] [Space]
    private Stars _stars;

    [SerializeField] [Space]
    private Btn _btnClose, _btnUnlock, _btnBuild;

    [SerializeField] [Space]
    private TMP_Text _txtTitle, _txtAmountMaster, _txtAmountStrength, _txtPrice, _txtBuildTime;




    private void OnEnable()
    {
        _btnClose.onSelect += Close;
        _btnUnlock.onSelect += Unlock;
        _btnBuild.onSelect += Build;
    }

    private void OnDisable()
    {
        _btnClose.onSelect -= Close;
        _btnUnlock.onSelect -= Unlock;
        _btnBuild.onSelect -= Build;
    }

    public void Display(TankProperties tankProperties, bool isUnlocked)
    {
        if (MyPhotonNetwork.IsOfflineMode)
            return;

        _img.sprite = tankProperties._iconTank;

        _stars.Display(tankProperties._starsCount);

        _txtTitle.text = tankProperties._tankName;

        _txtPrice.text = tankProperties._getItNowPrice.ToString();

        _txtBuildTime.text = Converter.HhMMSS(tankProperties._initialBuildHours, tankProperties._initialBuildMinutes, tankProperties._initialBuildSeconds);

        for (int i = 0; i < tankProperties._requiredItems.Length; i++)
        {
            _txtAmountMaster.text = tankProperties._requiredItems[0]._number.ToString();

            _txtAmountStrength.text = tankProperties._requiredItems[1]._number.ToString();
        }

        SetCanvasGroupActive(true);
    }

    private void Close()
    {
        SetCanvasGroupActive(false);
    }

    private void Unlock()
    {

    }

    private void Build()
    {

    }

    private void SetCanvasGroupActive(bool isActive)
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, isActive);
        GlobalFunctions.CanvasGroupActivity(_canvasGroupSubTabUnlock, isActive);
    }
}
