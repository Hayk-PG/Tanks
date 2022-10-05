using UnityEngine;

public class PlayerNewHUD : PlayerHUD
{
    protected override void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _mainCanvasGroup = GetComponent<CanvasGroup>();
        _isGroundedChecker = Get<IsGroundedChecker>.FromChild(transform.parent.gameObject);
    }

    protected override void OnEnable()
    {
        if (_isGroundedChecker != null)
            _isGroundedChecker.OnGrounded += EnablePlayerHUD;
    }

    protected override void OnDisable()
    {
        if (_isGroundedChecker != null)
            _isGroundedChecker.OnGrounded -= EnablePlayerHUD;
    }

    protected override void EnablePlayerHUD()
    {
        MainCanvasGroupActivity(true);
    }
}
