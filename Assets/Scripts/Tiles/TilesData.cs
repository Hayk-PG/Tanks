using System.Collections.Generic;
using UnityEngine;

public class TilesData : MonoBehaviour
{
    /// <summary>
    /// 0:T 1:M 2:L 3:R 4:TL 5:TR 6:LS 7:RS 8:RTL 9:RL
    /// </summary>
    public GameObject[] TilesPrefabs;

    public Dictionary<Vector3, GameObject> TilesDict = new Dictionary<Vector3, GameObject>();

    public float Size { get; set; } = 0.5f;
}
