﻿using System;
using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class GameManager : MonoBehaviourPun
{
    public static bool MasterPlayerReady { get; set; }
    public static bool SecondPlayerReady { get; set; }
    public static bool IsGameStarted { get; internal set; }
    public bool IsGameFinished { get; private set; }
    public bool IsGameRunning => IsGameStarted && !IsGameFinished;
    public Action OnInstantiateOfflinePlayers { get; set; }
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
        OnInstantiateOfflinePlayers?.Invoke();
    }

    private void StartOnlineMode()
    {
        if (MyPhotonNetwork.AmPhotonViewOwner(photonView))
        {
            PhotonNetwork.RaiseEvent(EventInfo._code_InstantiatePlayers, EventInfo._content_InstantiatePlayers, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
        }
    }
}
