using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;


public class HUDComponentPlayerName : MonoBehaviour
{
    [SerializeField] 
    private TMP_Text _txtPlayerName;

    [SerializeField] [Space]
    private TurnState _turnState;

    private bool _isPlayerNameAssigned;



    private void OnEnable()
    {
        if (MyPhotonNetwork.IsOfflineMode)
            GameSceneObjectsReferences.TurnController.OnTurnChanged += delegate { AssignTxtPlayerNameInOfflineMode(); };

        if (!MyPhotonNetwork.IsOfflineMode)
            GameSceneObjectsReferences.TurnController.OnTurnChanged += delegate { AssignTxtPlayerInOnlineMode(); };
    }

    private void OnDisable()
    {
        if (MyPhotonNetwork.IsOfflineMode)
            GameSceneObjectsReferences.TurnController.OnTurnChanged -= delegate { AssignTxtPlayerNameInOfflineMode(); };

        if (!MyPhotonNetwork.IsOfflineMode)
            GameSceneObjectsReferences.TurnController.OnTurnChanged -= delegate { AssignTxtPlayerInOnlineMode(); };
    }

    private void AssignTxtPlayerNameInOfflineMode()
    {
        if (!_isPlayerNameAssigned)
        {
            _txtPlayerName.text = _turnState == TurnState.Player1 ? "Player1" : "Player2";

            _isPlayerNameAssigned = true;
        }
    }

    private void AssignTxtPlayerInOnlineMode()
    {
        if (!_isPlayerNameAssigned)
        {
            GlobalFunctions.Loop<Player>.Foreach(PhotonNetwork.PlayerList, player => 
            {
                if (player.IsMasterClient && _turnState == TurnState.Player1)
                    _txtPlayerName.text = player.NickName;

                if (!player.IsMasterClient && _turnState == TurnState.Player2)
                    _txtPlayerName.text = player.NickName;
            });

            _isPlayerNameAssigned = true;
        }
    }
}
