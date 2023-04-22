using System.Collections;
using UnityEngine;
using Coffee.UIEffects;

public class MapSelector : MonoBehaviour, IReset
{
    [SerializeField]
    private Btn _btn;

    [SerializeField] [Space]
    private BtnTxt _btnTxt;

    [SerializeField] [Space]
    private Btn_Icon _bntIconLock;

    [SerializeField] [Space]
    private UIGradient _uiGradient;

    [SerializeField] [Space]
    private UIShiny _uiShiny;

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

    public event System.Action onRandomMapSelected;





    private void OnEnable()
    {
        _btn.onSelect += () => { _onSelect(); };

        _btn.onDeselect += () => 
        { 
            SetUiGradientActive(true);

            SetUiShinyActive(false);
        };
    }

    public void AssignMapSelector(bool isRandomMapSelector, Maps maps) => _onSelect = isRandomMapSelector ? () => { SelectRandomMap(maps); } : SelectMap;

    private void SetUiGradientActive(bool isActive)
    {
        if (_uiGradient.enabled == isActive)
            return;

        _uiGradient.enabled = isActive;
    }

    private void SetUiShinyActive(bool isActive)
    {
        if (_uiShiny.enabled == isActive)
            return;

        _uiShiny.enabled = isActive;
    }

    private void SelectRandomMap(Maps maps)
    {
        int randomMap = Random.Range(0, maps.All.Length);

        Data.Manager.SetMap(randomMap);

        MenuTabs.Tab_HomeOffline.DisplayMapName(maps.All[randomMap]);

        StartCoroutine(DelayBeforeDeselect());

        onRandomMapSelected?.Invoke();
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

        SetUiGradientActive(false);

        SetUiShinyActive(true);
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
