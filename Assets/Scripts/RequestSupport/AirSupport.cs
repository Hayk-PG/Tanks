using System.Collections;
using UnityEngine;

public class AirSupport : MonoBehaviour, ITurnController
{
    [SerializeField] private Bomber _bomber;

    private Rigidbody _rb;
    private MapPoints _mapPoints;
    private MainCameraController _mainCameraController;
    private float StartPointX
    {
        get
        {
            return _mapPoints.HorizontalMin - 2;
        }
    }
    private float EndPointX
    {
        get
        {
            return _mapPoints.HorizontalMax + 2;
        }
    }
    public TurnController TurnController { get; set; }


    private void Awake()
    {
        _mapPoints = FindObjectOfType<MapPoints>();
        _mainCameraController = FindObjectOfType<MainCameraController>();
        TurnController = FindObjectOfType<TurnController>();
    }

    private Vector3 StartPosition(PlayerTurn playerTurn)
    {
        return playerTurn.MyTurn == TurnState.Player1 ? 
            new Vector3(StartPointX, 5, 0) :
            new Vector3(EndPointX, 5, 0);
    }

    private Quaternion StartRotation(PlayerTurn playerTurn)
    {
        return playerTurn.MyTurn == TurnState.Player1 ? 
            Quaternion.Euler(-90, 90, 0) : Quaternion.Euler(-90, -90, 0);
    }

    public void Call(out Bomber bomber, PlayerTurn ownerTurn, IScore ownerScore, Vector3 dropPoint)
    {
        _bomber.transform.position = StartPosition(ownerTurn);
        _bomber.transform.rotation = StartRotation(ownerTurn);
        _bomber.MinX = StartPointX;
        _bomber.MaxX = EndPointX;
        _bomber.OwnerScore = ownerScore;
        _bomber.OwnerTurn = ownerTurn;
        _bomber.DropPoint = dropPoint;
        bomber = _bomber;
        TurnController.SetNextTurn(TurnState.Other);
        StartCoroutine(ActivateBomber(ownerTurn));
    }

    private IEnumerator ActivateBomber(PlayerTurn playerTurn)
    {
        StartCoroutine(PlayAirSirenSound());
        yield return new WaitForSeconds(1);

        _bomber.gameObject.SetActive(true);
        _mainCameraController.CameraOffset(playerTurn, _rb, _bomber.transform.position.y - 2, 2);
    }

    private IEnumerator PlayAirSirenSound()
    {
        SoundController.MusicSRCVolume(SoundController.MusicVolume.Down);
        SoundController.PlaySound(3, 0, out float clipLength);
        yield return new WaitForSeconds(clipLength);
        SoundController.MusicSRCVolume(SoundController.MusicVolume.Up);
    }
}
