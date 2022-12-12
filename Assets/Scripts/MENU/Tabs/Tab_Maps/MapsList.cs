using UnityEngine;

public class MapsList : MonoBehaviour
{  
    [SerializeField] private Transform _content;    
    [SerializeField] private Maps _maps;
    private MapSelector[] _mapSelectors;
    private Btn[] _btns;


    private void Awake()
    {
        _btns = _content.GetComponentsInChildren<Btn>();        
    }

    private void Start()
    {
        AddMapSelectors();
    }

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
            _mapSelectors[i] = Get<MapSelector>.From(_btns[i].gameObject);

            if (i < _maps.All.Length)
                _mapSelectors[i].Initialize(_maps.All[i], i);
        }
    }

    private void DeselectOtherBtns(Btn btn)
    {
        GlobalFunctions.Loop<Btn>.Foreach(_btns, b =>
        {
            if (b != btn)
                b.Deselect();
        });
    }
}
