using System;
using UnityEngine;

public class PropsTypeButton : AmmoTypeButton
{
    public Action<PropsTypeButton> OnClickPropsTypeButton { get; set; }

    public override void OnClickButton()
    {
        OnClickPropsTypeButton?.Invoke(this);
    }
}
