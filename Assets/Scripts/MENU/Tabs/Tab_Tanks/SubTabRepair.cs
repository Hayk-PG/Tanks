using UnityEngine;
using TMPro;


public class SubTabRepair : MonoBehaviour
{
    [SerializeField] [Space]
    private CanvasGroup _canvasGroup, _canvasGroupSubTabRepair;

    [SerializeField] [Space]
    private TMP_Text _txtWarning, txtAmountMaster, _txtAmountStrength;

    [SerializeField] [Space]
    private Btn _btnRepair, _btnClose;

    private string _warning = "Commander, your tank has sustained damage in battle and is in need of repairs. " +
                              "To fully utilize its capabilities, repairs are necessary. " +
                              "However, if you choose to continue without repairing, your tank can still be operated, but its starting health will not be at maximum. " +
                              "Please make a decision before proceeding with the next turn.";

    private string _warningCritical = "Attention Commander, your tank has sustained critical damage and is currently inoperable. " +
                                           "To continue using your tank, it must be repaired. " +
                                           "Please select the repair option from the menu and allocate the necessary resources. " +
                                           "The sooner repairs are made, the sooner your tank can return to full functionality.";





    private void OnEnable()
    {
        _btnRepair.onSelect += Repair;

        _btnClose.onSelect += Close;
    }

    private void OnDisable()
    {
        _btnRepair.onSelect -= Repair;

        _btnClose.onSelect -= Close;
    }

    public void Display(TankProperties tankProperties)
    {
        _txtWarning.text = Random.Range(0, 2) < 1 ? _warning : _warningCritical;

        SetCanvasGroupActive(true);
    }

    private void Repair()
    {

    }

    private void Close()
    {
        SetCanvasGroupActive(false);
    }

    private void SetCanvasGroupActive(bool isActive)
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, isActive);
        GlobalFunctions.CanvasGroupActivity(_canvasGroupSubTabRepair, isActive);
    }
}
