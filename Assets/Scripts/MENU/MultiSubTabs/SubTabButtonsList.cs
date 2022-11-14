using UnityEngine;

public class SubTabButtonsList : MonoBehaviour, IReset
{
    [SerializeField] private SubTabsButton subTabButtonFirst;
    [SerializeField] private SubTabsButton[] _subTabsButtonsOfflineMode;
    [SerializeField] private SubTabsButton[] _subTabsButtonOnlineMode;
    
 

    public void SetDefault()
    {
        subTabButtonFirst.Select();
        SetSubTabsButtonActivityInOfflineMode();
        SetSubTabsButtonActivityInOnlineMode();
    }

    private void SetSubTabsButtonActivityInOfflineMode()
    {
        if (_subTabsButtonsOfflineMode == null)
            return;

        if (MyPhotonNetwork.IsOfflineMode)
            GlobalFunctions.Loop<SubTabsButton>.Foreach(_subTabsButtonsOfflineMode, subTabButton => { subTabButton.gameObject.SetActive(false); });
    }

    private void SetSubTabsButtonActivityInOnlineMode()
    {
        if (_subTabsButtonOnlineMode == null)
            return;

        if (!MyPhotonNetwork.IsOfflineMode)
            GlobalFunctions.Loop<SubTabsButton>.Foreach(_subTabsButtonOnlineMode, subTabButton => { subTabButton.gameObject.SetActive(false); });
    }
}
