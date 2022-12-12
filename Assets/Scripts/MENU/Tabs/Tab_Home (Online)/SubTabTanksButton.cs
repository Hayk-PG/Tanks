using System;
using UnityEngine;

public class SubTabTanksButton : MonoBehaviour
{
    private SubTabsButton _subTabButton;

    public event Action onOpenTanksTab;



    private void Awake()
    {
        _subTabButton = Get<SubTabsButton>.From(gameObject);
    }

    private void OnEnable()
    {
        _subTabButton.onSelect += delegate { onOpenTanksTab?.Invoke(); };
    }

    private void OnDisable()
    {
        _subTabButton.onSelect -= delegate { onOpenTanksTab?.Invoke(); };
    }
}
