using UnityEngine;

public class Tab_Tanks : Tab_Base<Tab_StartGame>
{
    private void OnEnable()
    {
        _object.onPlayOffline += OpenTab;
    }

    private void OnDisable()
    {
        _object.onPlayOffline -= OpenTab;
    }

    public override void OpenTab()
    {
        base.OpenTab();
    }
}
