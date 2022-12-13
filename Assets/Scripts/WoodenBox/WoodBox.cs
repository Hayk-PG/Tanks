using UnityEngine;


public class WoodBox : MonoBehaviour
{
    [SerializeField]
    private WeaponProperties[] _newWeapon;

    private ParachuteWithWoodBoxController _woodBoxController;
    private ParachuteWithWoodBoxCollision _parachuteWithWoodBoxCollision;
    private WoodenBoxSerializer _woodBoxSerializer;
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
        _woodBoxSerializer = FindObjectOfType<WoodenBoxSerializer>();
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

        Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, SetInitValues, AllocateWoodBox);
    }

    private void OnEnable() => _parachuteWithWoodBoxCollision.onCollisionEnter += GetCollisions;

    private void OnDisable() => _parachuteWithWoodBoxCollision.onCollisionEnter -= GetCollisions;

    private void AllocateWoodBox() => _woodBoxSerializer.AllocateWoodBox();

    public void SetInitValues()
    {
        ContentIndex = Random.Range(0, ContentsCount);
        WeaponIndex = Random.Range(0, WeaponsCount);
    }

    private void GetCollisions(ParachuteWithWoodBoxCollision.CollisionData collisionData)
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
