using System.Collections;
using System.Collections.Generic;
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
