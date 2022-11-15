using UnityEngine;

[RequireComponent(typeof(Btn_Tank))]

public abstract class BaseBtnTankLogic<T> : MonoBehaviour, IReset where T: MonoBehaviour
{
    protected BaseTab_Tanks<T> _tabTanks;
    [SerializeField] protected Btn_Tank _btnTank;
    [SerializeField] Sprite _sprtButtonReleased;
    [SerializeField] protected Sprite _sprtButtonPressed;
    [SerializeField] protected Color _clrTankReleased;  
    [SerializeField] protected Color _clrTankPressed;
    


    protected virtual void Awake()
    {
        _tabTanks = FindObjectOfType<BaseTab_Tanks<T>>();
    }

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

        _btnTank.SpriteButton = _sprtButtonPressed;
        _btnTank.ImageTank.color = _clrTankPressed;
    }

    public void SetDefault()
    {
        _btnTank.SpriteButton = _sprtButtonReleased;
        _btnTank.ImageTank.color = _clrTankReleased;
    }
}
