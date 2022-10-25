using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class OnlineGameModeButton : MonoBehaviour
{
    public enum OnlineGameMode { Tournament, FindOrCreate, QuickMatch}

    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _textTitle;
    [SerializeField] private Image _icon;

    public OnlineGameMode _onlineGameMode;

    public event Action<OnlineGameMode> onClickOnlineGameModeButton;


    public void OnClick() 
    {
        onClickOnlineGameModeButton?.Invoke(_onlineGameMode);
    }

    public void SetButtonInteractability(bool isInteractable)
    {
        _button.interactable = isInteractable;
    }
}
