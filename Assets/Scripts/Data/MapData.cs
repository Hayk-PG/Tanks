using UnityEngine;

public partial class Data : MonoBehaviour
{
    [Header("Map")]
    [SerializeField] private int _mapIndex;

    public int MapIndex
    {
        get => _mapIndex;
        set => _mapIndex = value;
    }
}
