using UnityEngine;
using UnityEngine.UI;
using System.Linq;

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

    private bool IsMapSet
    {
        get => _map == null ? false : true;
    }
    
    private void Awake()
    {
        _selectMapButtons = transform.parent.GetComponentsInChildren<SelectMapButton>();
    }

    private void Start()
    {
        InitializeMapSizeText();
        MapAvailability(!IsMapSet);
    }

    private void Update()
    {
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(OnClickMapButton);
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
            SelectMap();

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

    private void SelectMap()
    {
        _mapIndex = _maps.All.ToList().FindIndex(map => map == _map);
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
}
