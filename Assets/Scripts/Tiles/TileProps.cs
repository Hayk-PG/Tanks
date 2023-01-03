using System;
using UnityEngine;
using UnityEngine.AddressableAssets;


public class TileProps : MonoBehaviour
{
    public enum PropsType { Sandbags, MetalCube, MetalGround, ExplosiveBarrels, AALauncher, All}
    public PropsType _propsType;

    private Tile _tile;

    [SerializeField] private Sandbags _sandBags;
    [SerializeField] private MetalCube _metalCube;
    [SerializeField] private MetalTile _metalGround;
    [SerializeField] private ExplosiveBarrels _explosiveBarrels;
   
    public Sandbags Sandbags => _sandBags;
    public MetalCube MetalCube => _metalCube;
    public MetalTile MetalGround => _metalGround;
    public ExplosiveBarrels ExplosiveBarrels => _explosiveBarrels;

    public event Action<bool, bool?> onAAProjectileLauncherActivity;



    private void Awake() => _tile = Get<Tile>.From(gameObject);

    private void OnTileProtection(bool isProtected)
    {
        if(_tile != null)
            _tile.IsProtected = isProtected;
    }

    private void SetSandbagsActivity(bool isActive, bool? isPlayer1)
    {
        Sandbags.gameObject.SetActive(isActive);

        if (isPlayer1 != null)
            Sandbags.SandbagsDirection(isPlayer1.Value);
    }

    private void SetArmoredCubeActivity(bool isActive) => MetalCube.gameObject.SetActive(isActive);

    private void SetArmoredTileActivity(bool isActive) => MetalGround.gameObject.SetActive(isActive);

    private void SetExplosiveBarrelsActivity(bool isActive) => ExplosiveBarrels.gameObject.SetActive(isActive);

    private void SetAAProjectileLaucnherActivity(bool isActive, bool? isPlayer1)
    {
        onAAProjectileLauncherActivity?.Invoke(isActive, isPlayer1);
    }

    public void ActiveProps(PropsType propsType, bool isActive, bool? isPlayer1)
    {
        switch (propsType)
        {
            case PropsType.Sandbags:
                SetSandbagsActivity(isActive, isPlayer1);
                break;

            case PropsType.MetalCube:
                SetArmoredCubeActivity(isActive);
                break;

            case PropsType.MetalGround:
                SetArmoredTileActivity(isActive);
                break;

            case PropsType.ExplosiveBarrels:
                SetExplosiveBarrelsActivity(isActive);
                SetArmoredTileActivity(isActive);
                break;

            case PropsType.AALauncher:
                SetAAProjectileLaucnherActivity(isActive, isPlayer1);
                SetArmoredTileActivity(isActive);
                break;

            case PropsType.All:
                SetSandbagsActivity(isActive, isPlayer1);
                SetExplosiveBarrelsActivity(isActive);
                SetAAProjectileLaucnherActivity(isActive, isPlayer1);
                SetArmoredCubeActivity(isActive);
                SetArmoredTileActivity(isActive);
                break;
        }

        OnTileProtection(isActive);
    }
}


