using UnityEngine;

public partial class Data : MonoBehaviour
{
    [Header("Map")]
    [SerializeField] private int _mapIndex;
    [SerializeField] private bool _isWindOn;

    public int MapIndex
    {
        get => _mapIndex;
        set => _mapIndex = value;
    }
    public bool IsWindOn
    {
        get => _isWindOn;
        set => _isWindOn = value;
    }
}
