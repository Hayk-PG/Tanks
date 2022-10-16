using UnityEngine;


public class WoodBox : MonoBehaviour
{
    [SerializeField]
    private WeaponProperties[] _newWeapon;

    private ParachuteWithWoodBoxController _woodBoxController;
    private ParachuteWithWoodBoxCollision _parachuteWithWoodBoxCollision;
    private Tab_WoodboxContent _tabWoodboxContent;
    private TurnController _turnController;
    private NewWeaponFromWoodBox _newWeaponFromWoodBox;

    private IWoodBoxContent[] _iWoodBoxContents;
    private IWoodBoxContent[] _iWoodBoxWeapons;
  
    private int ContentsCount
    {
        get => _iWoodBoxContents.Length + _iWoodBoxWeapons.Length;
    }
    private int WeaponsCount
    {
        get => _iWoodBoxWeapons.Length;
    }
    public int ContentIndex { get; set; }
    public int WeaponIndex { get; set; }


    private void Awake()
    {
        _woodBoxController = Get<ParachuteWithWoodBoxController>.From(gameObject);
        _parachuteWithWoodBoxCollision = Get<ParachuteWithWoodBoxCollision>.From(gameObject);
        _tabWoodboxContent = FindObjectOfType<Tab_WoodboxContent>();
        _turnController = FindObjectOfType<TurnController>();
        _newWeaponFromWoodBox = FindObjectOfType<NewWeaponFromWoodBox>();

        _iWoodBoxContents = new IWoodBoxContent[]
        {
            new AddScoreContent(),
            new AddMoreHPContent(),
            new AddMoreShellsContent(),
            new GiveTurnContent(_turnController)
        };

        _iWoodBoxWeapons = new IWoodBoxContent[]
        {
            new AddNewWeaponContent(_newWeapon[0], _newWeaponFromWoodBox)
        };
    }

    private void OnEnable()
    {
        _woodBoxController.OnInitialized += OnWoodBoxControllerInitialized;
        _parachuteWithWoodBoxCollision.OnCollision += OnCollision;
    }

    private void OnDisable()
    {
        _woodBoxController.OnInitialized -= OnWoodBoxControllerInitialized;
        _parachuteWithWoodBoxCollision.OnCollision -= OnCollision;
    }

    private void OnWoodBoxControllerInitialized(WoodenBoxSerializer woodBoxSerializer)
    {
        woodBoxSerializer.WoodBox = this;

        ContentIndex = Random.Range(0, ContentsCount);
        WeaponIndex = Random.Range(0, WeaponsCount);
    }

    private void OnCollision(ParachuteWithWoodBoxCollision.CollisionData collisionData)
    {
        if (collisionData._tankController != null)
        {
            OnContent(collisionData._tankController);
        }
    }

    public void OnContent(TankController tankController)
    {      
        if (ContentIndex < _iWoodBoxContents.Length)
        {
            _iWoodBoxContents[ContentIndex].Use(tankController, _tabWoodboxContent);
        }
        else
        {
            _iWoodBoxWeapons[WeaponIndex].Use(tankController, _tabWoodboxContent);
        }
    }
}
