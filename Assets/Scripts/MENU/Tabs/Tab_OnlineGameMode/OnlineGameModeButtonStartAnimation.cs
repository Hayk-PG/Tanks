using System;
using UnityEngine;

public class OnlineGameModeButtonStartAnimation : MonoBehaviour
{
    [SerializeReference] private Animator _animator;
    [SerializeField] private OnlineGameModeButtonStartAnimation _nextButton;

    private Tab_OnlineGameMode tabOnlineGameMode;

    [SerializeField] private bool _isFirstButton;
    [SerializeField] private bool _isLastButton;
    private string _animationTrigger = "start";

    public event Action<bool> onButtonsInteractability;


    private void Awake()
    {
        GetOnlineGameModeTab();
    }

    private void OnEnable()
    {
        if (tabOnlineGameMode != null)
            tabOnlineGameMode.OnTabOpened += PlayStartAnimation;
    }

    private void OnDisable()
    {
        if (tabOnlineGameMode != null)
            tabOnlineGameMode.OnTabOpened -= PlayStartAnimation;
    }

    private void GetOnlineGameModeTab()
    {
        if (_isFirstButton)
            tabOnlineGameMode = FindObjectOfType<Tab_OnlineGameMode>();
    }

    public void PlayStartAnimation()
    {
        _animator.SetTrigger(_animationTrigger);
        onButtonsInteractability?.Invoke(false);
    }

    public void PlayNextButtonStartAnimation()
    {
        if (_nextButton != null)
            _nextButton.PlayStartAnimation();

        if (_isLastButton)
            onButtonsInteractability?.Invoke(true);
    }
}
