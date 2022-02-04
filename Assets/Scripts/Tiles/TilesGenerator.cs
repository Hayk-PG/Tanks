using System.Collections.Generic;
using UnityEngine;

public class TilesGenerator : MonoBehaviour
{
    /// <summary>
    /// 0:T 1:M 2:L 3:R 4:TL 5:TR 6:LS 7:RS 8:RTL 9:RL
    /// </summary>
    public GameObject[] TilesPrefabs;

    [SerializeField] float xStartPoint, xEndPoint, yStartPoint, yEndPoint;
    [SerializeField] float size;

    internal Dictionary<Vector3, GameObject> TilesDict = new Dictionary<Vector3, GameObject>();

    public float XStartPoint { get => xStartPoint; }
    public float XEndPoint { get => xEndPoint; }
    public float YStartPoint { get => yStartPoint; }
    public float YEndPoint { get => yEndPoint; }
    public float Size { get => size; }




    void Awake()
    {
        size = TilesPrefabs[0].GetComponentInChildren<MeshRenderer>().bounds.size.x;
    }

    void Start()
    {
        GenerateTiles();
    }

    public void GenerateTiles()
    {
        for (float x = xStartPoint; x < xEndPoint; x += size)
        {
            for (float y = yStartPoint; y > yEndPoint; y -= size)
            {
                int index = x != xStartPoint && x != xEndPoint - size && y == yStartPoint ? 0 :
                            x != xStartPoint && x != xEndPoint - size && y != yStartPoint ? 1 :
                            x == xStartPoint && y != yStartPoint ? 2 :
                            x == xEndPoint - size && y != yStartPoint ? 3 :
                            x == xStartPoint && y == yStartPoint ? 4 :
                            x == xEndPoint - size && y == yStartPoint ? 5 :
                            6;

                int r = Random.Range(0, 10);

                if(y == yStartPoint && r > 2)
                {
                    Create(index, x, y);
                }
                if (y != yStartPoint && !TilesDict.ContainsKey(new Vector3(x, y + size, 0)) && r > 2 && r <= 5)
                {
                    Create(index, x, y);
                }
                if (y != yStartPoint && TilesDict.ContainsKey(new Vector3(x, y + size, 0)) && y != yEndPoint + size || y == yEndPoint + size)
                {
                    Create(index, x, y);
                }
            }
        }
    }

    void Create(int index, float x, float y)
    {
        if(!TilesDict.ContainsKey(new Vector3(x, y, 0)))
        {
            GameObject tile = Instantiate(TilesPrefabs[index], new Vector3(x, y, 0), Quaternion.identity, transform);
            tile.name = TilesPrefabs[index].name;
            TilesDict.Add(tile.transform.position, tile);
        }
    }
}
