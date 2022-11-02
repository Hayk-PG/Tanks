using UnityEngine;
using UnityEngine.UI;

public class MatchmakeLastStep : MonoBehaviour, IReset
{
    [SerializeField] private Button _confirmButton;
    private SubTab _subTab;
    private MatchmakeLastSubTab _lastSubTab;


    private void Awake()
    {
        _lastSubTab = Get<MatchmakeLastSubTab>.From(gameObject);
        _subTab = Get<SubTab>.From(gameObject);
    }

    private void OnEnable()
    {
        _lastSubTab._onConfirmReady += SetEverythingUp;
        _subTab._onActivity += GetSubTabActivity;
    }

    private void OnDisable()
    {
        _lastSubTab._onConfirmReady -= SetEverythingUp;
        _subTab._onActivity -= GetSubTabActivity;
    }

    private void SetEverythingUp()
    {
        _confirmButton.interactable = true;
    }

    private void GetSubTabActivity(bool isActive)
    {
        if (!isActive)
        {
            SetDefault();
        }
    }

    public void SetDefault()
    {
        _confirmButton.interactable = false;
    }
}
