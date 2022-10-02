using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TopBarProfileButton : MonoBehaviour
{
    [SerializeField]
    private UnityEvent OnClickNo;

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
            _messageTypes.CouldntOpenProfileTabMessage(OnClickNo);
        }
        else
        {
            OnClickTopBarProfileButton?.Invoke();            
        }
    }
}
