using Photon.Pun;
using UnityEngine;

public class GroundSlam : MonoBehaviourPun
{
    [SerializeField] private GameObject _groundSlamVfx;
    private CameraShake _cameraShake;
    private TurnController _turnController;

    private TankController _tankController;
    private IScore _iScore;


    private void Awake()
    {
        _turnController = FindObjectOfType<TurnController>();
        _cameraShake = FindObjectOfType<CameraShake>();
    }

    public void OnGroundSlam(Vector3 position)
    {
        if (_groundSlamVfx.activeInHierarchy) return;

        _groundSlamVfx.transform.position = position;
        _groundSlamVfx.SetActive(true);
        _cameraShake.Shake();

        OnScore();
    }

    private void OnScore()
    {
        if (!MyPhotonNetwork.IsOfflineMode && MyPhotonNetwork.AmPhotonViewOwner(photonView))
            photonView.RPC("GetScoreRPC", RpcTarget.AllViaServer, (int)_turnController._previousTurnState);
        else if (MyPhotonNetwork.IsOfflineMode)
            GetScore((int)_turnController._previousTurnState);
    }
    
    private void GetScore(int previousTurnIndex)
    {
        _iScore = GlobalFunctions.ObjectsOfType<PlayerTurn>.Find(turn => turn.MyTurn == (TurnState)previousTurnIndex)?.GetComponent<IScore>();
        _iScore?.GetScore(100, null);
    }

    [PunRPC]
    private void GetScoreRPC(int previousTurnIndex)
    {
        _tankController = GlobalFunctions.ObjectsOfType<PlayerTurn>.Find(turn => turn.MyTurn == (TurnState)previousTurnIndex)?.GetComponent<TankController>();

        if (_tankController != null && _tankController.BasePlayer != null && _tankController.BasePlayer.photonView.IsMine)
            GetScore(previousTurnIndex);

        print("GroundSlam" + "/" + previousTurnIndex);
    }
}
