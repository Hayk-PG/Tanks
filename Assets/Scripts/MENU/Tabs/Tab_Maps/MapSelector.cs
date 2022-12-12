using UnityEngine;

public class MapSelector : MonoBehaviour, IReset
{
    [SerializeField] private Btn _btn;
    [SerializeField] private BtnTxt _btnTxt;
    [SerializeField] private Btn_Icon _bntIconLock;
    private Map _map;
    private int _mapIndex;

    public bool IsLocked
    {
        get => _bntIconLock.gameObject.activeInHierarchy;
        private set
        {
            _bntIconLock.gameObject.SetActive(value);
            _btn.IsInteractable = !value;
        }
    }


    private void OnEnable()
    {
        _btn.onSelect += Select;
    }

    private void OnDisable()
    {
        _btn.onSelect -= Select;
    }

    private void Select()
    {
        Data.Manager.SetMap(_mapIndex);
    }

    public void Initialize(Map map, int index)
    {
        GetMap(map);
        GetMapIndex(index);
        Lock(false);
    }

    private void GetMap(Map map) => _map = map;
    private void GetMapIndex(int index)
    {
        _mapIndex = index;
        _btnTxt.SetButtonTitle((_mapIndex + 1).ToString());
    }
    public void Lock(bool isLocked) => IsLocked = isLocked;

    public void SetDefault()
    {
        if (_mapIndex == Data.Manager.MapIndex && _map != null)
            _btn.Select();
    }
}
