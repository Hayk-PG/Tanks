using UnityEngine;

public class Btn_DropBoxSelectionPanelElement : MonoBehaviour
{
    [SerializeField]
    private Btn _btn;

    [SerializeField] [Space]
    private BtnTxt[] _btnTxts;

    public Btn Btn => _btn;
    public BtnTxt[] BtnTxts => _btnTxts;
 }
