using UnityEngine;

public class Tab_BomberControl : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private HUDMainTabsActivity _hudMainActivity;


    private void Awake()
    {
        _canvasGroup = Get<CanvasGroup>.From(gameObject);
        _hudMainActivity = FindObjectOfType<HUDMainTabsActivity>();
    }

    public void TabBomberControlActivity(bool isActive)
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, isActive);
        _hudMainActivity.CanvasGroupsActivity(!isActive);
    }
}
