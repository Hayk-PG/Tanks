using UnityEngine;
using System;

public class Tab_RocketController : MonoBehaviour, IHudTabsObserver, IEndGame
{
    [SerializeField]
    private CanvasGroup _canvasGroup;

    [SerializeField] [Space]
    private FixedJoystick _fixedJoystick;

    public Vector3 Direction
    {
        get
        {
            return new Vector2(_fixedJoystick.Horizontal, _fixedJoystick.Vertical);
        }
    }

    public event Action<bool> onActivity;




    public void SetActivity(bool isActive)
    {
        GameSceneObjectsReferences.HudTabsHandler.RequestTabActivityPermission(this, HudTabsHandler.HudTab.TabRocketController, isActive); 
    }

    public void Execute(bool isActive)
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, isActive);

        GameSceneObjectsReferences.Controllers.SetControllersActive(true);
    }

    public void OnGameEnd(object[] data = null)
    {
        SetActivity(false);
    }

    public void WrapUpGame(object[] data = null)
    {
        
    }
}
