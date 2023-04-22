using UnityEngine;
using UnityEngine.UI;

public class SelectableTanksStats : MonoBehaviour
{
    [SerializeField] private Image _imgMobilityFillAmount;
    [SerializeField] private Image _imgProtectionFillAmount;
    [SerializeField] private Image _imgFirepowerFillAmount;

    private BaseBtnTankLogic[] _btnTanksLogics;



    private void Awake()
    {
        _btnTanksLogics = transform.parent.GetComponentsInChildren<BaseBtnTankLogic>(true);
    }

    private void OnEnable() 
    {
        GlobalFunctions.Loop<BaseBtnTankLogic>.Foreach(_btnTanksLogics, btnTankLogic => { btnTankLogic.onTankSelected += GetSelectedTankInfo; });
    }

    private void OnDisable()
    {
        GlobalFunctions.Loop<BaseBtnTankLogic>.Foreach(_btnTanksLogics, btnTankLogic => { btnTankLogic.onTankSelected -= GetSelectedTankInfo; });
    }

    private void GetSelectedTankInfo(TankProperties selectedTankProperties)
    {
        SetFillAmount(_imgMobilityFillAmount, selectedTankProperties._mobility);
        SetFillAmount(_imgProtectionFillAmount, selectedTankProperties._protection);
        SetFillAmount(_imgFirepowerFillAmount, selectedTankProperties._firePower);
    }

    private void SetFillAmount(Image imgFillAmount, float value)
    {
        imgFillAmount.fillAmount = value;
    }
}
