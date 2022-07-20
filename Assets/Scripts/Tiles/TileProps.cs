using System;
using UnityEngine;


public class TileProps : MonoBehaviour
{
    public enum PropsType { Sandbags, MetalCube, MetalGround}
    public PropsType _propsType;

    private Tile _tile;

    [SerializeField] private Sandbags _sandBags;
    [SerializeField] private MetalCube _metalCube;
    [SerializeField] private MetalTile _metalGround;
    
    public Sandbags Sandbags => _sandBags;
    public MetalCube MetalCube => _metalCube;
    public MetalTile MetalGround => _metalGround;


    private void OnEnable()
    {
        _tile = Get<Tile>.From(gameObject);
    }

    private void OnTileProtection(bool isProtected)
    {
        _tile.IsProtected = isProtected;
    }

    public void ActiveProps(PropsType propsType, bool isActive, bool? isPlayer1)
    {
        switch (propsType)
        {
            case PropsType.Sandbags:
                Sandbags.gameObject.SetActive(isActive);
                if (isPlayer1 != null)
                    Sandbags.SandbagsDirection(isPlayer1.Value);
                break;

            case PropsType.MetalCube:
                MetalCube.gameObject.SetActive(isActive);
                break;

            case PropsType.MetalGround:
                transform.GetChild(0).gameObject.SetActive(!isActive);
                MetalGround.gameObject.SetActive(isActive);
                break;
        }

        OnTileProtection(true);
    }
}


