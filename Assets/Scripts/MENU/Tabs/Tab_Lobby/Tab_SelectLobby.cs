using System;

public class Tab_SelectLobby : Tab_Base, ITab_Base
{
    private ITab_Base _itabBase;
    private MyPhotonCallbacks _myPhotonCallbacks;

    public event Action<ITab_Base> onSendTab;
    


    protected override void Awake()
    {
        base.Awake();
        _itabBase = Get<ITab_Base>.From(gameObject);
        _myPhotonCallbacks = FindObjectOfType<MyPhotonCallbacks>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        MenuTabs.Tab_HomeOnline.onGoForward += OpenTab;
        MenuTabs.Tab_Tanks.onSendTab += OpenTab;
        _myPhotonCallbacks._OnLeftRoom += OpenTab;
    }

    protected override void OnDisable()
    {
        base.OnEnable();
        MenuTabs.Tab_HomeOnline.onGoForward -= OpenTab;
        MenuTabs.Tab_Tanks.onSendTab -= OpenTab;
        _myPhotonCallbacks._OnLeftRoom -= OpenTab;
    }

    protected override void GoForward() => onSendTab?.Invoke(_itabBase);

    private void OpenTab(ITab_Base tab_Base)
    {
        if (tab_Base == _itabBase)
            base.OpenTab();
    }
}
