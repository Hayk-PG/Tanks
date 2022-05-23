using System;
using UnityEngine;
using UnityEngine.UI;

public class TopBarProfileButton : MonoBehaviour
{
    private Button _button;
    private MessageTypes _messageTypes;
    public Action OnClickTopBarProfileButton { get; set; }


    private void Awake()
    {
        _button = Get<Button>.From(gameObject);
        _messageTypes = FindObjectOfType<MessageTypes>();
    }

    private void Update()
    {
        OnClick();
    }

    private void OnClick()
    {
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(AttemptToOpenProfileTab);
    }

    private void AttemptToOpenProfileTab()
    {
        if (MyPhotonNetwork.IsOfflineMode)
        {
            _messageTypes.CouldntOpenProfileTabMessage();
        }
        else
        {
            OnClickTopBarProfileButton?.Invoke();
            
        }
    }
}
