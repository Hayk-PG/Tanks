using UnityEngine;

public class PlayerNewHUD : PlayerHUD
{
    protected override void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _mainCanvasGroup = GetComponent<CanvasGroup>();
    }

    protected override void OnEnable()
    {
        
    }

    protected override void OnDisable()
    {
        
    }

    protected override void EnablePlayerHUD()
    {
        MainCanvasGroupActivity(true);
    }
}
