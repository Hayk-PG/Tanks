using System.Collections.Generic;
using UnityEngine;

public class TilesData : MonoBehaviour
{
    [SerializeField] private Tile[] _prefabs;
    /// <summary>
    /// 0:T, 1:M, 2:L, 3:R, 4:TL, 5:TR, 6:LS, 7:RS, 8:TRL, 9:RL, 10:B, 11:BL, 12:BR, 13:RBL, 14:TLB, 15:TRB, 16:TB, 17:RTLB
    /// </summary>
    public Tile[] TilesPrefabs
    {
        get => _prefabs;
    }
    public Dictionary<Vector3, GameObject> TilesDict { get; set; } = new Dictionary<Vector3, GameObject>();
    public float Size { get; set; } = 0.5f;
}
