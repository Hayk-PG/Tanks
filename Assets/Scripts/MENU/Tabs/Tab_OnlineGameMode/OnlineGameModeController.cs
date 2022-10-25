using System;
using UnityEngine;

public class OnlineGameModeController : MonoBehaviour
{
    [SerializeField] private OnlineGameModeButton[] _onlineGameModeButtons;
    [SerializeField] private GameObject _raycastBlocker;
    private OnlineGameModeButtonStartAnimation[] onlineGameModeButtonsStartAnimations;

    public event Action onSelectTournametMode;


    private void Awake()
    {
        GetButtonsStartAnimations();
    }

    private void OnEnable()
    {
        GlobalFunctions.Loop<OnlineGameModeButton>.Foreach(_onlineGameModeButtons, button => { button.onClickOnlineGameModeButton += ClickedButtons; });
        GlobalFunctions.Loop<OnlineGameModeButtonStartAnimation>.Foreach(onlineGameModeButtonsStartAnimations, startAnimation => { startAnimation.onButtonsInteractability += SetButtonsInteractabilities; });
    }

    private void OnDisable()
    {
        GlobalFunctions.Loop<OnlineGameModeButton>.Foreach(_onlineGameModeButtons, matchMakerButton => { matchMakerButton.onClickOnlineGameModeButton -= ClickedButtons; });
        GlobalFunctions.Loop<OnlineGameModeButtonStartAnimation>.Foreach(onlineGameModeButtonsStartAnimations, matchMakerButtonStartAnimation => { matchMakerButtonStartAnimation.onButtonsInteractability -= SetButtonsInteractabilities; });
    }

    private void GetButtonsStartAnimations()
    {
        onlineGameModeButtonsStartAnimations = new OnlineGameModeButtonStartAnimation[_onlineGameModeButtons.Length];

        for (int i = 0; i < onlineGameModeButtonsStartAnimations.Length; i++)
        {
            onlineGameModeButtonsStartAnimations[i] = Get<OnlineGameModeButtonStartAnimation>.From(_onlineGameModeButtons[i].gameObject);
        }
    }

    private void ClickedButtons(OnlineGameModeButton.OnlineGameMode gameMode)
    {
        _raycastBlocker.SetActive(true);

        if (gameMode == OnlineGameModeButton.OnlineGameMode.Tournament)
            onSelectTournametMode?.Invoke();

        if(gameMode == OnlineGameModeButton.OnlineGameMode.FindOrCreate)
            MyPhoton.JoinLobby("", Photon.Realtime.LobbyType.Default);
    }

    private void SetButtonsInteractabilities(bool isInteractable)
    {
        GlobalFunctions.Loop<OnlineGameModeButton>.Foreach(_onlineGameModeButtons, matchMakerButton => { matchMakerButton.SetButtonInteractability(isInteractable); });

        if (!isInteractable)
            _raycastBlocker.SetActive(false);
    }
}
