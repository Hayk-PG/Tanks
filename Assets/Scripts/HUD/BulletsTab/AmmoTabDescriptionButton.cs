using System;
using UnityEngine;

public class AmmoTabDescriptionButton : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup _canvasGroup;

    [SerializeField] [Space]
    private Btn _btn;

    [SerializeField] [Space]
    private Btn_Icon _btnIcon;

    [SerializeField] [Space]
    private Sprite[] _sprts;

    public bool IsActive { get; private set; }

    public event Action<bool> onDescriptionActivity;




    private void OnEnable() => _btn.onSelect += OnSelect;
    private void OnDisable() => _btn.onSelect -= OnSelect;

    private void OnSelect()
    {
        IsActive = !IsActive;

        _btnIcon.ChangeIconHolder(IsActive ? _sprts[0] : _sprts[1]);

        onDescriptionActivity?.Invoke(IsActive);
    }
}
