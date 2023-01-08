using System;
using UnityEngine;
using UnityEngine.AddressableAssets;


public class TileProps : MonoBehaviour
{
    public enum PropsType { MetalCube, MetalGround, ExplosiveBarrels, AALauncher, All}
    public PropsType _propsType;

    private Tile _tile;

    [SerializeField] private MetalCube _metalCube;
    [SerializeField] private MetalTile _metalGround;
    [SerializeField] private ExplosiveBarrels _explosiveBarrels;
   
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
                SetExplosiveBarrelsActivity(isActive);
                SetAAProjectileLaucnherActivity(isActive, isPlayer1);
                SetArmoredCubeActivity(isActive);
                SetArmoredTileActivity(isActive);
                break;
        }

        OnTileProtection(isActive);
    }
}


