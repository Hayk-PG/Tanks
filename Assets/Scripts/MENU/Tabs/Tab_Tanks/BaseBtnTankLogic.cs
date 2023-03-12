using UnityEngine;

[RequireComponent(typeof(Btn_Tank))]

public abstract class BaseBtnTankLogic : MonoBehaviour, IReset 
{
    protected BaseTab_Tanks _tabTanks;

    [SerializeField]
    protected Btn_Tank _btnTank;

    [SerializeField] [Space]
    protected Color _clrBtnReleased, _clrBtnPressed;

    [SerializeField] [Space]
    protected Color _clrTankReleased, _clrTankPressed;

    public event System.Action<TankProperties> onTankSelected;




    protected abstract void Awake();

    protected virtual void OnEnable()
    {
        _btnTank._onAutoSelect += AutoSelect;
        _btnTank._onClick += Select;
    }

    protected virtual void OnDisable()
    {
        _btnTank._onAutoSelect -= AutoSelect;
        _btnTank._onClick -= Select;
    }

    protected virtual void SetScrollRectPosition(int horizontalGroupsLength)
    {
        int horizontalGroupIndex = transform.parent.GetSiblingIndex() + 1;

        float value = Mathf.InverseLerp(horizontalGroupsLength, 1, horizontalGroupIndex);

        _tabTanks.CustomScrollRect.SetNormalizedPosition(value);
    }

    protected virtual void AutoSelect(int relatedTankIndex, int horizontalGroupsLength)
    {
        Select(relatedTankIndex);

        SetScrollRectPosition(horizontalGroupsLength);
    }

    protected abstract void SaveTankIndex(int relatedTankIndex);

    protected virtual void Select(int relatedTankIndex)
    {
        SaveTankIndex(relatedTankIndex);

        GlobalFunctions.Loop<IReset>.Foreach(_tabTanks.GetComponentsInChildren<IReset>(true), iReset =>
        {
            if (iReset.GetType() != typeof(CustomScrollRect))
                iReset.SetDefault();
        });

        _btnTank.ColorBtn = _clrBtnPressed;

        _btnTank.ImageTank.color = _clrTankPressed;

        _tabTanks.SubTabUnlock?.Display(_btnTank.TankProperties, _btnTank.IsLocked);

        _tabTanks.SubTabRepair?.Display(_btnTank.TankProperties);

        onTankSelected?.Invoke(Data.Manager.AvailableTanks[relatedTankIndex]);
    }

    public void SetDefault()
    {
        _btnTank.ColorBtn = _clrBtnReleased;
        _btnTank.ImageTank.color = _clrTankReleased;
    }
}
