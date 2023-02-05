using UnityEngine;

public class PlayerNewHUD : PlayerHUD
{
    private void Awake()
    {
        EnablePlayerHUD();
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
