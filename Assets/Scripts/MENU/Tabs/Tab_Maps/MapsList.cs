using System.Collections;
using UnityEngine;

public class MapsList : MonoBehaviour, IReset
{  
    [SerializeField]
    private Transform _content;
    
    [SerializeField] [Space]
    private Maps _maps;

    [SerializeField] [Space]
    private CustomScrollRect _customScrollRect;

    private MapSelector[] _mapSelectors;

    private Btn[] _btns;




    private void Awake() => _btns = _content.GetComponentsInChildren<Btn>();

    private void Start() => AddMapSelectors();

    private void OnEnable()
    {
        GlobalFunctions.Loop<Btn>.Foreach(_btns, btn => 
        { 
            btn.onSelect += delegate { DeselectOtherBtns(btn); }; 
        });
    }

    private void OnDisable()
    {
        GlobalFunctions.Loop<Btn>.Foreach(_btns, btn => 
        { 
            btn.onSelect += delegate { DeselectOtherBtns(btn); };
        });
    }

    private void AddMapSelectors()
    {
        _mapSelectors = new MapSelector[_btns.Length];

        for (int i = 0; i < _mapSelectors.Length; i++)
        {
            int mapIndex = i - 1;

            _mapSelectors[i] = Get<MapSelector>.From(_btns[i].gameObject);

            if (i == 0)
            {
                _mapSelectors[i].Initialize();
                _mapSelectors[i].AssignMapSelector(true, _maps);

                continue;
            }

            if (mapIndex < _maps.All.Length)
            {
                _mapSelectors[i].Initialize(_maps.All[mapIndex], mapIndex);
                _mapSelectors[i].AssignMapSelector(false, null);
            }
        }
    }

    private IEnumerator SetCustomScrollRectNormalizedPosition()
    {
        float horizontalBarsCount = _content.childCount;
        float currentHorizontalBarIndex = (Data.Manager.MapIndex / 3) < 0 ? 0 : Data.Manager.MapIndex / 3;
        float normalizedPosition = Mathf.InverseLerp(horizontalBarsCount, 1, currentHorizontalBarIndex + 1);

        yield return null;

        _customScrollRect.SetNormalizedPosition(normalizedPosition);
    }

    private void DeselectOtherBtns(Btn btn)
    {
        GlobalFunctions.Loop<Btn>.Foreach(_btns, b =>
        {
            if (b != btn)
                b.Deselect();
        });
    }

    public void SetDefault()
    {
        StartCoroutine(SetCustomScrollRectNormalizedPosition());
    }
}
