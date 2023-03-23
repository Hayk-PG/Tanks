using System.Collections;
using UnityEngine;

public class MapSelector : MonoBehaviour, IReset
{
    [SerializeField]
    private Btn _btn;

    [SerializeField] [Space]
    private BtnTxt _btnTxt;

    [SerializeField] [Space]
    private Btn_Icon _bntIconLock;

    private Map _map;

    private System.Action _onSelect;

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





    private void OnEnable() => _btn.onSelect += () => { _onSelect(); };

    private void OnDisable() => _btn.onSelect -= () => { _onSelect(); };

    public void AssignMapSelector(bool isRandomMapSelector, Maps maps)
    {
        _onSelect = isRandomMapSelector ? () => { SelectRandomMap(maps); } : SelectMap;
    }

    private void SelectRandomMap(Maps maps)
    {
        int randomMap = Random.Range(0, maps.All.Length);

        Data.Manager.SetMap(randomMap);

        MenuTabs.Tab_HomeOffline.DisplayMapName(maps.All[randomMap]);

        StartCoroutine(DelayBeforeDeselect());
    }

    private IEnumerator DelayBeforeDeselect()
    {
        yield return null;

        _btn.Deselect();
    }

    private void SelectMap()
    {
        Data.Manager.SetMap(_mapIndex);

        MenuTabs.Tab_HomeOffline.DisplayMapName(_map);
    }

    public void Initialize()
    {
        _btnTxt.SetButtonTitle("?");

        Lock(false);
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
            StartCoroutine(AutoSelect());
    }

    private IEnumerator AutoSelect()
    {
        yield return null;

        _btn.Select();
    }
}
