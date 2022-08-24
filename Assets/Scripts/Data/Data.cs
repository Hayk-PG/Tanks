using System;
using System.Collections.Generic;
using UnityEngine;

public partial class Data : MonoBehaviour
{
    public static Data Manager { get; private set; }
    private MyPlayfab _myPlayfab;

    private void Awake()
    {
        Instance();
        _myPlayfab = Get<MyPlayfab>.From(gameObject);
    }

    private void Instance()
    {
        if (Manager != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Manager = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnEnable()
    {
        if (_myPlayfab != null)
            _myPlayfab.OnUpdateReadOnlyData += OnUpdateReadOnlyData;
    }

    private void OnDisable()
    {
        if (_myPlayfab != null)
            _myPlayfab.OnUpdateReadOnlyData -= OnUpdateReadOnlyData;
    }

    public void OnUpdateReadOnlyData(string playfabId, Action<string, Dictionary<string, string>> updateUserDataRequest)
    {
        CreateTanksReadOnlyData(playfabId, updateUserDataRequest);
    }
}
