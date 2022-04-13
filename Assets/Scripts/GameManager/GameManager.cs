using System;
using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class GameManager : MonoBehaviourPun
{
    public bool IsGameStarted { get; private set; }
    public bool IsGameFinished { get; private set; }
    public bool IsGameRunning => IsGameStarted && !IsGameFinished;
    public float TimeToStartTheGame { get; private set; }
    public Action OnGameStarted { get; set; }
    

    private void Start()
    {
        StartCoroutine(StartGameCoroutine());
    }

    private IEnumerator StartGameCoroutine()
    {
        yield return new WaitForSeconds(1);

        Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode && !IsGameStarted, StartOfflineMode, StartOnlineMode);
    }

    private void StartOfflineMode()
    {
        IsGameStarted = true;
        OnGameStarted?.Invoke();
    }

    private void StartOnlineMode()
    {
        if (photonView.IsMine)
        {
            PhotonNetwork.RaiseEvent(EventInfo._code_InstantiatePlayers, EventInfo._content_InstantiatePlayers, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
        }
    }
}
