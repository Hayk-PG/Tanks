using System;
using UnityEngine;
using UnityEngine.UI;

public class Chat : MonoBehaviour
{
    [SerializeField] private InputField _inputField;
    [SerializeField] private Text _textPrefab;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Transform _list;
    [SerializeField] private Color[] _colors;

    public string ChatText
    {
        get => _inputField.text;
        set => _inputField.text = value;
    }
    public Action<string> OnSend { get; set; }
    public Action<bool> OnTextInstantiated { get; set; }


    public void OnClickToSend()
    {
        OnSend?.Invoke(ChatText);
        ChatText = "";
    }

    public void InstantiateText(string senderName, string chatText, int playerIndex)
    {
        Text text = Instantiate(_textPrefab, _list);
        text.color = _colors[playerIndex];
        text.text = senderName + "| " + chatText;       
        OnTextInstantiated?.Invoke(_canvasGroup.interactable);
    }
}
