using System;
using UnityEngine;
using TMPro;

public class BaseAnnouncer : MonoBehaviour
{
    [SerializeField] 
    protected TMP_Text[] texts;

    [SerializeField] [Space]
    protected GameManager _gameManager;

    [SerializeField] [Space]
    protected TurnController _turnController;

    protected SoundController _soundController;
    
    public Action OnGameStartAnnouncement { get; set; }




    protected virtual void Awake() => _soundController = FindObjectOfType<SoundController>();

    protected virtual void OnEnable() => _turnController.OnTurnChanged += OnTurnChanged;

    protected virtual void OnDisable() => _turnController.OnTurnChanged -= OnTurnChanged;

    public virtual void TextAnnouncement(int index, string text, bool isActive)
    {
        texts[index].text = text;
        texts[index].gameObject.SetActive(isActive);
    }

    protected virtual void OnTurnChanged(TurnState turnState)
    {

    }
}
