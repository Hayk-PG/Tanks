using System;
using UnityEngine;

public class ShootButton : MonoBehaviour
{
    public Action<bool> OnPointer { get; set; }
    public Action<bool> OnClick { get; set; }

    [SerializeField] bool p;


    public void OnButtonClicked()
    {
        OnClick?.Invoke(true);
    }

    public void OnPointerDown()
    {
        OnPointer?.Invoke(true);

        p = true;
    }

    public void OnPointerUp()
    {
        OnPointer?.Invoke(false);

        p = false;
    }
}
