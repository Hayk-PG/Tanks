using UnityEngine;

public class Btn_DropBoxSelectionPanelElement : MonoBehaviour
{
    [SerializeField]
    private Btn _btn;

    [SerializeField]
    private Btn_Icon _btnIconStar;

    [SerializeField] [Space]
    private BtnTxt[] _btnTxts;

    public Btn Btn => _btn;

    public Btn_Icon Btn_IconStar => _btnIconStar;

    public BtnTxt[] BtnTxts => _btnTxts;
 }
