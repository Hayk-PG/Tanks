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
        _btnTank._onAutoSelect += AutoSelect;
        _btnTank._onClick += Select;
    }

    private void OnDisable()
    {
        _btnTank._onAutoSelect -= AutoSelect;
        _btnTank._onClick -= Select;
    }

    private void SetScrollRectPosition(int horizontalGroupsLength)
    {
        int horizontalGroupIndex = transform.parent.GetSiblingIndex() + 1;
        float value = Mathf.InverseLerp(horizontalGroupsLength, 1, horizontalGroupIndex);
        _tabTanks.CustomScrollRect.SetNormalizedPosition(value);
    }

    private void AutoSelect(int relatedTankIndex, int horizontalGroupsLength)
    {
        Select(relatedTankIndex);
        SetScrollRectPosition(horizontalGroupsLength);
    }

    private void Select(int relatedTankIndex)
    {
        Data.Manager.SetData(new Data.NewData { SelectedTankIndex = relatedTankIndex });
        GlobalFunctions.Loop<IReset>.Foreach(_tabTanks.GetComponentsInChildren<IReset>(true), iReset => { iReset.SetDefault(); });

        _btnTank.SpriteButton = _sprtButtonPressed;
        _btnTank.ImageTank.color = _clrTankPressed;
    }

    public void SetDefault()
    { 
        _btnTank.SpriteButton = _sprtButtonReleased;
        _btnTank.ImageTank.color = _clrTankReleased;
    }
   
    public void Deselect()
    {
        
    }
}
