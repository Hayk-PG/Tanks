using UnityEngine;

public class SubTabButtonsList : MonoBehaviour, IReset
{
    [SerializeField] private SubTabsButton _firsSubTabButton;
    [SerializeField] private SubTabsButton _generalSubTabButton;
    [SerializeField] private SubTabsButton _rateSubTabButton;
  

    public void SetDefault()
    {
        _firsSubTabButton.Click();

        ButtonsInteractability(!MyPhotonNetwork.IsOfflineMode);
    }

    private void ButtonsInteractability(bool isInteractable)
    {
        _generalSubTabButton.IsInteractable = isInteractable;
        _rateSubTabButton.IsInteractable = isInteractable;
    }
}
