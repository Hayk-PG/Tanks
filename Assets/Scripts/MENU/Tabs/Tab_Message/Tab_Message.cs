using System;
using UnityEngine;
using UnityEngine.UI;

public enum MessageType { Error, Neutral, Success}

public class Tab_Message : Tab_Base<MyPhotonCallbacks>
{
    [SerializeField] private MessageType _messageType;
    [SerializeField] private MessageTitle[] _messageTitles;
    [SerializeField] private Button[] _buttonActions;
    [SerializeField] private Text _text;
    
    public Action OnYes { get; private set; }
    public Action OnNo { get; private set; }


    private void Update()
    {
        OnClick();
    }

    public void OnMessage(Action OnYes, Action OnNo, MessageType messageType, string[] messages)
    {
        this.OnYes = OnYes;
        this.OnNo = OnNo;
        _messageTitles[(int)messageType].Print(messages[0]);
        _text.text = messages[1];
        base.OpenTab();
    }
    private void OnClick()
    {
        for (int i = 0; i < _buttonActions.Length; i++)
        {
            _buttonActions[i].onClick.RemoveAllListeners();
            Conditions<bool>.Compare(i == 0, OnClickYesButton, OnClickNoButton);
        }
    }
    private void OnClickYesButton() => _buttonActions[0].onClick.AddListener(() => OnYes?.Invoke());
    private void OnClickNoButton() => _buttonActions[1].onClick.AddListener(() => OnNo?.Invoke());
}
