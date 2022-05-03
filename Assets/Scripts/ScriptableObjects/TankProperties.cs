using UnityEngine;

[CreateAssetMenu(fileName = "New tank", menuName = "Tank")]
public class TankProperties : ScriptableObject
{
    public int _tankIndex;

    [Header("UI")]
    public Sprite _iconTank;

    [Header("Prefab")]
    public TankController _tank;
}
