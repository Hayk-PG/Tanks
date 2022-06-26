using System;
using UnityEngine;
using UnityEngine.UI;

public class ChatRematchMessage : MonoBehaviour
{
    [SerializeField] private Text _textName;
    [SerializeField] private Button _buttonAccept;
    [SerializeField] private Button _buttonDecline;

    public Action<bool> OnRespond { get; set; }

    public string Name
    {
        get => _textName.text;
        set => _textName.text = value;
    }


    public void OnClickButtonAccept()
    {
        DisableButtons();
        OnRespond?.Invoke(true);      
    }

    public void OnClickButtonDecline()
    {
        DisableButtons();
        OnRespond?.Invoke(false);
    }

    private void DisableButtons()
    {
        _buttonAccept.interactable = false;
        _buttonDecline.interactable = false;
    }
}
