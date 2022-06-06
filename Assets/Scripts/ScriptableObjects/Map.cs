using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable objects/Map/New map")]
public class Map : ScriptableObject
{
    public enum Type { Small, Medium, Large}

    [SerializeField] private Type _mapType;
    [SerializeField] private Texture2D _mapTexture;
    [SerializeField] private Sprite _mapImage;

    public Type MapType => _mapType;
    public Texture2D Texture => _mapTexture;
    public Sprite MapImage => _mapImage;
}
