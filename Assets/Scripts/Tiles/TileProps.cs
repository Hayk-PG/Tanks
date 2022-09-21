using UnityEngine;


public class TileProps : MonoBehaviour
{
    public enum PropsType { Sandbags, MetalCube, MetalGround, All}
    public PropsType _propsType;

    private Tile _tile;

    [SerializeField] private Sandbags _sandBags;
    [SerializeField] private MetalCube _metalCube;
    [SerializeField] private MetalTile _metalGround;
    
    public Sandbags Sandbags => _sandBags;
    public MetalCube MetalCube => _metalCube;
    public MetalTile MetalGround => _metalGround;


    private void Awake()
    {
        _tile = Get<Tile>.From(gameObject);
    }

    private void OnTileProtection(bool isProtected)
    {
        if(_tile != null)
            _tile.IsProtected = isProtected;
    }

    private void OnSandbags(bool isActive, bool? isPlayer1)
    {
        Sandbags.gameObject.SetActive(isActive);

        if (isPlayer1 != null)
            Sandbags.SandbagsDirection(isPlayer1.Value);
    }

    private void OnArmoredCube(bool isActive)
    {
        MetalCube.gameObject.SetActive(isActive);
    }

    private void OnArmoredTile(bool isActive)
    {
        transform.GetChild(0).gameObject.SetActive(!isActive);
        MetalGround.gameObject.SetActive(isActive);
    }

    public void ActiveProps(PropsType propsType, bool isActive, bool? isPlayer1)
    {
        switch (propsType)
        {
            case PropsType.Sandbags:
                OnSandbags(isActive, isPlayer1);
                break;

            case PropsType.MetalCube:
                OnArmoredCube(isActive);
                break;

            case PropsType.MetalGround:
                OnArmoredTile(isActive);
                break;

            case PropsType.All:
                OnSandbags(isActive, isPlayer1);
                OnArmoredCube(isActive);
                OnArmoredTile(isActive);
                break;
        }

        OnTileProtection(isActive);
    }
}


