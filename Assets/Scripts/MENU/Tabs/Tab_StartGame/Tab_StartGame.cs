﻿using System;
using System.Collections;
using UnityEngine;

public class Tab_StartGame : Tab_Base
{
    [SerializeField]
    private Btn _btnOffline, _btnOnline;

    public event Action onPlayOffline;
    public event Action onPlayOnline;


    protected override void OnEnable()
    {
        MenuTabs.Tab_Initialize.onOpenTabStartGame += OpenTab;

        _btnOffline.onSelect += SelectOffline;
        _btnOnline.onSelect += SelectOnline;
    }

    protected override void OnDisable()
    {
        MenuTabs.Tab_Initialize.onOpenTabStartGame -= OpenTab;

        _btnOffline.onSelect -= SelectOffline;
        _btnOnline.onSelect -= SelectOnline;
    }
   
    public override void OpenTab()
    {
        MyPhoton.LeaveRoom();
        MyPhoton.LeaveLobby();
        MyPhoton.Disconnect();

        base.OpenTab();
    }

    public void SelectOffline()
    {
        _tabLoading.Open();

        StartCoroutine(Execute(true, onPlayOffline));
    }

    public void SelectOnline()
    {
        _tabLoading.Open();

        StartCoroutine(Execute(false, onPlayOnline));
    }

    private IEnumerator Execute(bool isOfflineMode, Action onPlay)
    {
        yield return new WaitForSeconds(0.5f);

        MyPhotonNetwork.ManageOfflineMode(isOfflineMode);

        onPlay?.Invoke();
    }
}
