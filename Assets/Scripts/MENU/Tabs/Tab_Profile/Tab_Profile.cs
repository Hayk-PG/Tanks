using UnityEngine;

public class Tab_Profile : Tab_Base<MyPhotonCallbacks>
{
    private TopBarProfileButton[] _topBarProfileButton;


    protected override void Awake()
    {
        base.Awake();
        _topBarProfileButton = FindObjectsOfType<TopBarProfileButton>();
    }

    private void OnEnable()
    {
        GlobalFunctions.Loop<TopBarProfileButton>.Foreach(_topBarProfileButton, profileButton => 
        {
            profileButton.OnClickTopBarProfileButton += OpenTab;
        });
    }

    private void OnDisable()
    {
        GlobalFunctions.Loop<TopBarProfileButton>.Foreach(_topBarProfileButton, profileButton =>
        {
            profileButton.OnClickTopBarProfileButton -= OpenTab;
        });
    }

    public override void OpenTab()
    {
        GlobalFunctions.CanvasGroupActivity(CanvasGroup, true);
    }

    public void CloseTab()
    {
        GlobalFunctions.CanvasGroupActivity(CanvasGroup, false);
    }
}
