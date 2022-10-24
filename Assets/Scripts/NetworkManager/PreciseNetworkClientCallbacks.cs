using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class PreciseNetworkClientCallbacks : MonoBehaviour
{
    [SerializeField] private ClientState _previousClientState;
    [SerializeField] private ClientState _currentClientState;

    public ClientState PreviousClientState { get => _previousClientState; private set => _previousClientState = value; }
    public ClientState CurrentClientState { get => _currentClientState; private set => _currentClientState = value; }


    public event Action<ClientState, ClientState> onNetworkClientState;


    private void Start()
    {
        StartCoroutine(CheckCurrentNetworkClientState());
    }

    public void GetCurrentNetworkClientState()
    {
        if(CurrentClientState != PhotonNetwork.NetworkClientState)
        {
            PreviousClientState = CurrentClientState;            
            CurrentClientState = PhotonNetwork.NetworkClientState;
            onNetworkClientState?.Invoke(PreviousClientState, CurrentClientState);
        } 
    }

    private IEnumerator CheckCurrentNetworkClientState()
    {
        while (true)
        {
            GetCurrentNetworkClientState();
            yield return new WaitForSeconds(1);
        }
    }
}
