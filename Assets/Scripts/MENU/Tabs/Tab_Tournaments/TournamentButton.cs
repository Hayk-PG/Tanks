using System;
using UnityEngine;
using UnityEngine.UI;

public class TournamentButton : MonoBehaviour
{
    private Button _button;
    private TitleGroupProperties _titleGroupProperties;

    public event Action<TitleGroupProperties> onPressTurnamentButton;


    private void Awake()
    {
        _button = Get<Button>.From(gameObject);
    }

    private void Update()
    {
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        onPressTurnamentButton?.Invoke(_titleGroupProperties);
    }

    public void Initialize(TitleGroupProperties titleGroupProperties)
    {
        _titleGroupProperties = titleGroupProperties;
    }
}
