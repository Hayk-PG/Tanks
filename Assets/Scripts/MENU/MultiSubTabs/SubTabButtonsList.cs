using UnityEngine;

public class SubTabButtonsList : MonoBehaviour, IReset
{
    [SerializeField] private SubTabsButton _firsSubTabButton;
    [SerializeField] private SubTabsButton[] _modeDependentButtons;
  

    public void SetDefault()
    {
        _firsSubTabButton.Click();

        ButtonsActivity(!MyPhotonNetwork.IsOfflineMode);
    }

    private void ButtonsActivity(bool isInteractable)
    {
        GlobalFunctions.Loop<SubTabsButton>.Foreach(_modeDependentButtons, dependentButton => { dependentButton.gameObject.SetActive(false); });
    }
}
