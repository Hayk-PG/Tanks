using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable objects/Map/New map")]
public class Map : ScriptableObject
{
    public enum Type { Small, Medium, Large}

    [SerializeField] 
    private Type _mapType;

    [SerializeField] [Space]
    private Texture2D _mapTexture;

    [SerializeField] [Space]
    private Sprite _mapImage;

    [SerializeField] [Space]
    private string _mapName;

    public Type MapType => _mapType;
    public Texture2D Texture => _mapTexture;
    public Sprite MapImage => _mapImage;
    public string MapName => _mapName;
}
