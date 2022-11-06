using System;
using UnityEngine.UI;

public class Tab_SelectedTanks : Tab_Base<Tab_StartGame>
{
    private TanksInfo _tanksInfo;

    public ScrollRect _scrollRect;

    public Action OnSingleGameTankSelected { get; set; }
    public event Action onOnlineModeTankSelected;
    public Action OnOnlineGameTankChanged { get; set; }
    


    protected override void Awake()
    {
        base.Awake();
        _tanksInfo = Get<TanksInfo>.From(gameObject);
    }

    private void OnEnable()
    {
        _object.onPlayOffline += OpenTab;
        //ExternalData.MyPlayfabRegistrationForm.onLogin += OpenTab;
    }

    private void OnDisable()
    {
        _object.onPlayOffline -= OpenTab;
        //ExternalData.MyPlayfabRegistrationForm.onLogin -= OpenTab;
    }

    public override void OpenTab()
    {
        _tanksInfo.ResetTanksInfoScreen();
        _scrollRect.verticalNormalizedPosition = 1;
        base.OpenTab();
    }

    public void OnClickSelectButton()
    {
        if (MyPhotonNetwork.IsOfflineMode)
        {
            OnSingleGameTankSelected?.Invoke();
        }
        else
        {
            Conditions<bool>.Compare(MyPhotonNetwork.IsInRoom, delegate { OnOnlineGameTankChanged?.Invoke(); }, delegate { onOnlineModeTankSelected?.Invoke(); });
        }
    }
}
