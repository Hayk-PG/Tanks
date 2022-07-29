using System;
using UnityEngine;
using TMPro;

public class BaseAnnouncer : MonoBehaviour
{
    [SerializeField] protected TMP_Text[] texts;

    protected GameManager _gameManager;
    protected SoundController _soundController;
    protected TurnController _turnController;

 
    public Action OnGameStartAnnouncement { get; set; }

    protected virtual void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _soundController = FindObjectOfType<SoundController>();
        _turnController = FindObjectOfType<TurnController>();
    }

    protected virtual void OnEnable()
    {
        _turnController.OnTurnChanged += OnTurnChanged;
    }

    protected virtual void OnDisable()
    {
        _turnController.OnTurnChanged -= OnTurnChanged;
    }

    public virtual void TextAnnouncement(int index, string text, bool isActive)
    {
        texts[index].text = text;
        texts[index].gameObject.SetActive(isActive);
    }

    protected virtual void OnTurnChanged(TurnState turnState)
    {

    }
}
