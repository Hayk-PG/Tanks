using UnityEngine;

public class SubTabButtonsList : MonoBehaviour, IReset
{
    [SerializeField] private SubTabsButton _firsSubTabButton;
    [SerializeField] private SubTabsButton[] _modeDependentButtons;
  

    public void SetDefault()
    {
        _firsSubTabButton.Click();

        ButtonsInteractability(!MyPhotonNetwork.IsOfflineMode);
    }

    private void ButtonsInteractability(bool isInteractable)
    {
        GlobalFunctions.Loop<SubTabsButton>.Foreach(_modeDependentButtons, dependentButton => { dependentButton.gameObject.SetActive(false); });
    }
}
