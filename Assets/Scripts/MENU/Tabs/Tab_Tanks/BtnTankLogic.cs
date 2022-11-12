using UnityEngine;

public class BtnTankLogic : MonoBehaviour, IReset
{
    private Tab_Tanks _tabTanks;
    [SerializeField] private Btn_Tank _btnTank;

    [SerializeField] private Sprite _sprtButtonReleased;
    [SerializeField] private Sprite _sprtButtonPressed;
    [SerializeField] private Color _clrTankReleased;
    [SerializeField] private Color _clrTankPressed;



    private void Awake()
    {
        _tabTanks = FindObjectOfType<Tab_Tanks>();
    }

    private void OnEnable()
    {
        _btnTank._onAutoSelect += delegate { Select(); };
    }

    private void OnDisable()
    {
        _btnTank._onAutoSelect -= delegate { Select(); };
    }

    private void Update()
    {
        _btnTank.Button.onClick.RemoveAllListeners();
        _btnTank.Button.onClick.AddListener(Click);
    }

    public void Select()
    {
        Data.Manager.SetData(new Data.NewData { SelectedTankIndex = _btnTank.RelaedTankIndex });
        GlobalFunctions.Loop<IReset>.Foreach(_tabTanks.GetComponentsInChildren<IReset>(true), iReset => { iReset.SetDefault(); });

        _btnTank.SpriteButton = _sprtButtonPressed;
        _btnTank.ImageTank.color = _clrTankPressed;
    }

    public void Deselect()
    { 
        _btnTank.SpriteButton = _sprtButtonReleased;
        _btnTank.ImageTank.color = _clrTankReleased;
    }

    public void Click()
    {
        Select();
    }

    public void SetDefault()
    {
        Deselect();
    }
}
