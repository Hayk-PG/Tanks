using UnityEngine;


public class WoodBox : MonoBehaviour
{
    [SerializeField]
    private WeaponProperties[] _newWeapon;

    [SerializeField] [Space]
    private ParachuteWithWoodBoxController _woodBoxController;

    [SerializeField] [Space]
    private ParachuteWithWoodBoxCollision _parachuteWithWoodBoxCollision;

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
        _iWoodBoxContents = new IWoodBoxContent[]
        {
            new AddScoreContent(),
            new AddMoreHPContent(),
            new AddMoreShellsContent(),
            new GiveTurnContent(GameSceneObjectsReferences.TurnController), 
            new AALauncherContent()
        };
        _iWoodBoxWeapons = new IWoodBoxContent[]
        {
            new AddNewWeaponContent(_newWeapon[0], GameSceneObjectsReferences.NewWeaponFromWoodBox)
        };

        Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, SetInitValues, AllocateWoodBox);
    }

    private void OnEnable() => _parachuteWithWoodBoxCollision.onCollisionEnter += GetCollisions;

    private void OnDisable() => _parachuteWithWoodBoxCollision.onCollisionEnter -= GetCollisions;

    private void AllocateWoodBox() => GameSceneObjectsReferences.WoodBoxSerializer.AllocateWoodBox();

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
            _iWoodBoxContents[ContentIndex].Use(tankController, GameSceneObjectsReferences.Tab_WoodboxContent);
        }
        else
        {
            _iWoodBoxWeapons[WeaponIndex].Use(tankController, GameSceneObjectsReferences.Tab_WoodboxContent);
        }
    }
}
