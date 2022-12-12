using UnityEngine;

public class OptionsQuit : OptionsController
{
    protected override void Select() => Application.Quit();
}
