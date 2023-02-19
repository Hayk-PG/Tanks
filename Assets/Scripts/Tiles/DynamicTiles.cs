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

    [SerializeField] [Space]
    private LayerMask _layerMask;

    private Color _clrLava = new Color(0.7450981f, 0.3254902f, 0.01568628f, 1f); //BE5304
    private Color _clrFire = new Color(0.7450981f, 0.509804f, 0.01568628f, 1f); //BE8204
    private Color _clrIce = new Color(0.1367925f, 1f, 0.9607843f, 1f); //2AADA7




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

        if (meshRenderer == null)
            return;

        GetDynamicType(meshRenderer, meshObj);
    }

    private void GetDynamicType(MeshRenderer meshRenderer, GameObject meshObj)
    {
        if (!GameSceneObjectsReferences.LevelCreator.DynamicTilesData.ContainsKey(transform.position))
            return;

        ChangeMaterialColor(meshRenderer, GameSceneObjectsReferences.LevelCreator.DynamicTilesData[transform.position]);

        ChangeMeshLayer(meshObj);
    }

    private void ChangeMaterialColor(MeshRenderer meshRenderer, DynamicTileType dynamicTileType)
    {
        GlobalFunctions.Loop<Material>.Foreach(meshRenderer.materials, material => 
        {
            material.color = dynamicTileType == DynamicTileType.Lava ? _clrLava :
                                dynamicTileType == DynamicTileType.Fire ? _clrFire : _clrIce;
        });
    } 

    private void ChangeMeshLayer(GameObject meshObj)
    {
        meshObj.layer = 0;
    }
    
    private int MaterialIndex()
    {
        return name == Names.B || name == Names.BL || name == Names.BR || name == Names.L || name == Names.R || name == Names.BL || name == Names.RBL ||
               name == Names.RL ? 1 : 0;
    }
}
