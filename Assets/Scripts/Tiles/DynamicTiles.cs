using UnityEngine;


public enum DynamicTileType 
{ 
    None, 
    Lava, 
    Fire, 
    Ice 
}

public class DynamicTiles : MonoBehaviour
{
    [SerializeField]
    private DynamicTileType _dynamicTileType;

    [SerializeField] [Space]
    private Tile _tile;


    private Color _clrLava = new Color(190f, 83f, 4f, 255f); //BE5304
    private Color _clrFire = new Color(190f, 130f, 4f, 255f); //BE8204
    private Color _clrIce = new Color(42f, 173f, 167f, 255f); //2AADA7

    public DynamicTileType Type
    {
        get => _dynamicTileType;
        private set => _dynamicTileType = value;
    }




    private void OnEnable()
    {
        _tile.onMeshInstantiated += OnMeshInstantiated;
    }

    private void OnDisable()
    {
        _tile.onMeshInstantiated -= OnMeshInstantiated;
    }

    private void OnMeshInstantiated(GameObject meshObj)
    {
        MeshRenderer meshRenderer = Get<MeshRenderer>.From(meshObj);

        if (meshRenderer == null || _dynamicTileType == DynamicTileType.None)
            return;

        ChangeMaterialColor(meshRenderer);
    }

    private void ChangeMaterialColor(MeshRenderer meshRenderer)
    {
        meshRenderer.material.color = _dynamicTileType == DynamicTileType.Lava ? _clrLava :
                                _dynamicTileType == DynamicTileType.Fire ? _clrFire : _clrIce;
    }    
}
