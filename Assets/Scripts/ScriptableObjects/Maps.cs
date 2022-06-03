using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable objects/Map/New maps container")]
public class Maps : ScriptableObject
{
    [SerializeField] private Map[] _maps;
    public Map[] All => _maps;
}
