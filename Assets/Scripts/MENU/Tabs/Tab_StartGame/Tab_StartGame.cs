using System;
using UnityEngine;

public class Tab_StartGame : Tab_Base<MyPhotonCallbacks>
{
    public Action OnStartPlayVsAi { get; set; }
    public Action OnStartPlayVsOtherPlayer { get; set; }


    public void OnClickVsAiButton()
    {
        OnStartPlayVsAi?.Invoke();
    }

    public void OnClickVsPlayerButton()
    {
        OnStartPlayVsOtherPlayer?.Invoke();
    }

    public override void OpenTab()
    {
        MyPhoton.Disconnect();
        base.OpenTab();
    }
}
