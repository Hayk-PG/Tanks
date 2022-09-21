using System;
using System.Collections.Generic;
using UnityEngine;

public class TilesData : MonoBehaviour
{
    [Serializable]
    public struct InactiveTiles
    {
        [SerializeField]
        private string _tileName;
        [SerializeField]
        private List<Tile> _tiles;

        public string TileName 
        {
            get => _tileName;
            set => _tileName = value;
        }
        public List<Tile> Tiles
        {
            get => _tiles;
            set => _tiles = value;
        }
    }
    public List<InactiveTiles> StoredInactiveTiles;   
   
    [SerializeField]
    private Tile[] _prefabs;
    [SerializeField]
    private List<Tile> _modifiableTiles;
    [SerializeField]
    private Transform _inactiveTilesContainer;
    
    public Tile[] TilesPrefabs
    {
        get => _prefabs;
    }
    public Dictionary<Vector3, GameObject> TilesDict { get; set; } = new Dictionary<Vector3, GameObject>();
    public List<Tile> ModifiableTiles
    {
        get => _modifiableTiles;
        set => _modifiableTiles = value;
    }
    public Transform IntactiveTilesContainer
    {
        get => _inactiveTilesContainer;
    }
    public float Size { get; set; } = 0.5f;
}
