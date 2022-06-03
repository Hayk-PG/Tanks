using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable objects/Map/New map")]
public class Map : ScriptableObject
{
    public enum Type { Small, Medium, Large}

    [SerializeField] private Type _mapType;
    [SerializeField] private Texture2D _mapTexture;

    public Type MapType => _mapType;
    public Texture2D Texture => _mapTexture;
}
