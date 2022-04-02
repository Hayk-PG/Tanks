using UnityEngine;

public class GroundSlam : MonoBehaviour
{
    [SerializeField]
    private GameObject _groundSlamVfx;

    private CameraShake _cameraShake;

    private TurnController _turnController;
    private IScore _iScore;


    private void Awake()
    {
        _turnController = FindObjectOfType<TurnController>();
        _cameraShake = FindObjectOfType<CameraShake>();
    }

    public void Vfx(Vector3 position)
    {
        if (_groundSlamVfx.activeInHierarchy) return;

        else
        {
            _groundSlamVfx.transform.position = position;
            _groundSlamVfx.SetActive(true);
            _cameraShake.Shake();
            GivePlayerPoints(position);
        }
    }

    private void GivePlayerPoints(Vector3 position)
    {
        _iScore = TurnController.Players.Find(turn => turn.MyTurn == _turnController._previousTurnState).GetComponent<IScore>();
        _iScore.GetScore(100, null);
    }
}
