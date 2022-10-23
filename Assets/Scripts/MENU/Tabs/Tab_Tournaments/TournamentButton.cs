using System;
using UnityEngine;
using UnityEngine.UI;

public class TournamentButton : MonoBehaviour
{
    private Button _button;
    private TitleProperties _titleGroupProperties;

    public event Action<TitleProperties> onPressTurnamentButton;


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

    public void Initialize(TitleProperties titleGroupProperties)
    {
        _titleGroupProperties = titleGroupProperties;
    }
}
