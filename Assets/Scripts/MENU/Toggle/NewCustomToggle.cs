using System;
using UnityEngine;
using UnityEngine.UI;

public class NewCustomToggle : MonoBehaviour
{
    [SerializeField]
    private Toggle _toggle;

    public event Action<bool> onValueChanged;


    public void OnValueChanged()
    {
        onValueChanged?.Invoke(_toggle.isOn);
    }
}
