using System;
using UnityEngine;

public class DropBombButton : MonoBehaviour
{
    public Action OnClick { get; set; }

    public void OnButtonClicked()
    {
        OnClick?.Invoke();
    }
}
