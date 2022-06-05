using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

public class SelectMapButton : MonoBehaviour
{
    [SerializeField] private Map _map;
    [SerializeField] private Maps _maps;
    [SerializeField] private Image _imageMap;
    [SerializeField] private Image _imageHighlight;
    [SerializeField] private Image _iconLock;
    [SerializeField] private Text _textMapSize;
    [SerializeField] private Button _button;
    [SerializeField] private Color[] _colors;
    [SerializeField] private Sprite[] _spriteHighlights;
    [SerializeField] private int _mapIndex;
    private SelectMapButton[] _selectMapButtons;
    private Tab_SelectMaps _tabSelectMap;
    private bool IsMapSet
    {
        get => _map == null ? false : true;
    }
    public int MapIndex => _mapIndex;




    private void Awake()
    {
        _selectMapButtons = transform.parent.GetComponentsInChildren<SelectMapButton>();
        _tabSelectMap = FindObjectOfType<Tab_SelectMaps>();
    }


    private void OnEnable()
    {
        _tabSelectMap.OnTabOpened += OnTabSelectedTabOpen;
    }

    private void OnDisable()
    {
        _tabSelectMap.OnTabOpened -= OnTabSelectedTabOpen;
    }

    private void Update()
    {
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(OnClickMapButton);       
    }

    private void OnTabSelectedTabOpen()
    {
        print("aaa");
        InitializeMapSizeText();
        MapAvailability(!IsMapSet);
        GetMapIndex();
        AutoSelect(IsMapSet && _mapIndex == 0);
    }

    private void InitializeMapSizeText()
    {
        if (IsMapSet) _textMapSize.text = _map.MapType == Map.Type.Small ? "Small" : _map.MapType == Map.Type.Medium ? "Medium" : "Large";
    }

    public void OnClickMapButton()
    {
        if (!IsLocked())
        {
            InteractWithButton(true);
            GetMapIndex();
            SetMapIndex();

            GlobalFunctions.Loop<SelectMapButton>.Foreach(_selectMapButtons, mapButton =>
            {
                if (mapButton != this)
                    mapButton.InteractWithButton(false);
            });
        }
    }

    public void InteractWithButton(bool isClicked)
    {
        if (!IsLocked())
        {
            _imageHighlight.sprite = isClicked ? _spriteHighlights[1] : _spriteHighlights[0];
        }
    }

    private void GetMapIndex()
    {
        _mapIndex = _maps.All.ToList().FindIndex(map => map == _map);
    }

    private void SetMapIndex()
    {
        if (MyPhotonNetwork.IsOfflineMode)
        {
            Data.Manager.MapIndex = _mapIndex;
        }
        else
        {

        }
    }

    public bool IsLocked()
    {
        return _iconLock.gameObject.activeInHierarchy && _imageHighlight.color == _colors[1] ? true : false;
    }

    public void MapAvailability(bool isLocked)
    {
        _iconLock.gameObject.SetActive(isLocked);

        if (isLocked)
        {
            _imageHighlight.sprite = _spriteHighlights[0];
            _imageHighlight.color = _colors[1];
        }
        else
        {
            _imageHighlight.sprite = _spriteHighlights[0];
            _imageHighlight.color = _colors[0];
        }
    }

    private void AutoSelect(bool canSelect)
    {
        if (canSelect)
        {
            PointerEventData ped = new PointerEventData(EventSystem.current);
            _button.OnPointerClick(ped);
            _button.OnSubmit(ped);
        }
    }
}
