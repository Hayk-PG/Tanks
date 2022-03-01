using UnityEngine;

public class Bomber : MonoBehaviour
{
    [SerializeField]
    private Transform _propeller;

    [SerializeField]
    private GameObject _bomb;

    private CameraMovement _cameraMovement;
    private TurnController _turnController;


    private void Awake()
    {
        _cameraMovement = FindObjectOfType<CameraMovement>();
        _turnController = FindObjectOfType<TurnController>();
    }

    private void FixedUpdate()
    {
        transform.Translate(-transform.forward * 2 * Time.fixedDeltaTime);
        _propeller.Rotate(Vector3.right, -1000 * Time.deltaTime);
    }

    public void DropBomb()
    {
        _bomb.transform.parent = null;
        _bomb.SetActive(true);
        _turnController.SetNextTurn(TurnState.Other);
        _cameraMovement.SetCameraTarget(_bomb.transform, 10, 2);
    }
}
