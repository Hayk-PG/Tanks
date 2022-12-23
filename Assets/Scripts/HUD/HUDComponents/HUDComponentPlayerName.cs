using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;


public class HUDComponentPlayerName : MonoBehaviour
{
    [SerializeField] private TMP_Text _txtPlayerName;
    [SerializeField] private TurnState _turnState;
    private TurnController _turnController;

    private bool _isPlayerNameAssigned;



    private void Awake()
    {
        _turnController = FindObjectOfType<TurnController>();
    }

    private void OnEnable()
    {
        if (MyPhotonNetwork.IsOfflineMode)
            _turnController.OnTurnChanged += delegate { AssignTxtPlayerNameInOfflineMode(); };

        if (!MyPhotonNetwork.IsOfflineMode)
            _turnController.OnTurnChanged += delegate { AssignTxtPlayerInOnlineMode(); };
    }

    private void OnDisable()
    {
        if (MyPhotonNetwork.IsOfflineMode)
            _turnController.OnTurnChanged -= delegate { AssignTxtPlayerNameInOfflineMode(); };

        if (!MyPhotonNetwork.IsOfflineMode)
            _turnController.OnTurnChanged -= delegate { AssignTxtPlayerInOnlineMode(); };
    }

    private void AssignTxtPlayerNameInOfflineMode()
    {
        if (!_isPlayerNameAssigned)
        {
            _txtPlayerName.text = _turnState == TurnState.Player1 ? "You" : "AI";
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
