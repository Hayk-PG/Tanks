﻿public class DataTransferToPlayerCustomProperty : Tab_Base<MyPhotonCallbacks>
{   
    private void OnEnable()
    {
        _object._OnJoinedLobby += DataTransition;
    }

    private void OnDisable()
    {
        _object._OnJoinedLobby -= DataTransition;
    }

    private void DataTransition()
    {
        CustomProperties.Player(MyPhotonNetwork.LocalPlayer, Keys.Level, Data.Manager.Level);
    }
}
