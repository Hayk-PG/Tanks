using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public delegate void OnEvents();

    public bool IsGameStarted { get; private set; }
    public bool IsGameFinished { get; private set; }
    public bool IsGameRunning => IsGameStarted && !IsGameFinished;
    public float TimeToStartTheGame { get; private set; }

    public event OnEvents OnGameStarted;


    void Update()
    {
        StartTheGame();
    }

    void StartTheGame()
    {
        if (!IsGameStarted)
        {
            TimeToStartTheGame += 1 * Time.deltaTime;

            if (TimeToStartTheGame >= 1)
            {
                IsGameStarted = true;
                OnGameStarted?.Invoke();
            }
        }
    }
}
