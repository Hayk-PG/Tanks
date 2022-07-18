using UnityEngine;

public class TileProps : MonoBehaviour
{
    [SerializeField] private Sandbags _sandBags;
    private Tile _tile;

    public Sandbags Sandbags => _sandBags;


    private void OnEnable()
    {
        _tile = Get<Tile>.From(gameObject);
    }

    public void OnSandbags(bool isActive, bool? isPlayer1)
    {
        Sandbags.gameObject.SetActive(isActive);
        _tile.HasSandbagsOnIt = true;

        if (isPlayer1 != null) Sandbags.SandbagsDirection(isPlayer1.Value);
    }
}
