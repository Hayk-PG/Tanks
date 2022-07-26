using System;
using UnityEngine;
using TMPro;

public class BaseAnnouncer : MonoBehaviour
{
    protected GameManager _gameManager;
    [SerializeField] protected TMP_Text[] texts;

    public Action OnGameStartAnnouncement { get; set; }

    protected virtual void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    protected virtual void OnEnable()
    {
        
    }

    protected virtual void OnDisable()
    {
        
    }

    public virtual void TextAnnouncement(int index, string text, bool isActive)
    {
        texts[index].text = text;
        texts[index].gameObject.SetActive(isActive);
    }
}
