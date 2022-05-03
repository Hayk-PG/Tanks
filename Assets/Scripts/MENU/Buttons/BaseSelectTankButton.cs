using UnityEngine;
using UnityEngine.UI;

public abstract class BaseSelectTankButton : MonoBehaviour
{
    [SerializeField] protected Image _iconTank;
    protected Button _button;
    protected Data _data;
    protected int _index;
    

    protected virtual void Awake()
    {
        _index = transform.GetSiblingIndex();
        _button = Get<Button>.From(gameObject);
        _data = FindObjectOfType<Data>();
    }

    protected virtual void Start()
    {
        SetIcon();
    }

    protected virtual void Update()
    {
        _button?.onClick.RemoveAllListeners();
        _button.onClick.AddListener(OnClickTankButton);
    }

    protected virtual bool IsIndexCorrect()
    {
        return _index < _data.AvailableTanks.Length;
    }

    protected virtual void SetIcon()
    {
        if (IsIndexCorrect()) _iconTank.sprite = _data.AvailableTanks[_index]._iconTank;
    }

    public abstract void OnClickTankButton();
}
