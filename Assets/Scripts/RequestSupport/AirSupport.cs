using System.Collections;
using UnityEngine;

public class AirSupport : MonoBehaviour
{
    [SerializeField] 
    private Bomber _bomber; 

    [SerializeField] [Space]
    private Rigidbody _rb;

    private float StartPointX
    {
        get
        {
            return GameSceneObjectsReferences.MapPoints.HorizontalMin - 2;
        }
    }
    private float EndPointX
    {
        get
        {
            return GameSceneObjectsReferences.MapPoints.HorizontalMax + 2;
        }
    }




    public void Call(PlayerTurn ownerTurn, IScore ownerScore, Vector3 dropPoint)
    {
        _bomber.transform.position = StartPosition(ownerTurn);
        _bomber.transform.rotation = StartRotation(ownerTurn);
        _bomber.MinX = StartPointX;
        _bomber.MaxX = EndPointX;
        _bomber.OwnerScore = ownerScore;
        _bomber.OwnerTurn = ownerTurn;
        _bomber.DropPoint = dropPoint;

        GameSceneObjectsReferences.TurnController.SetNextTurn(TurnState.Other);

        StartCoroutine(ActivateBomber(ownerTurn));
    }

    private Vector3 StartPosition(PlayerTurn playerTurn)
    {
        return playerTurn.MyTurn == TurnState.Player1 ? new Vector3(StartPointX, 5, 0) : new Vector3(EndPointX, 5, 0);
    }

    private Quaternion StartRotation(PlayerTurn playerTurn)
    {
        return playerTurn.MyTurn == TurnState.Player1 ? Quaternion.Euler(-90, 90, 0) : Quaternion.Euler(-90, -90, 0);   
    }

    private IEnumerator ActivateBomber(PlayerTurn playerTurn)
    {
        yield return StartCoroutine(PlayAirSirenSound());

        _bomber.gameObject.SetActive(true);

        _rb.velocity = transform.right * 2;

        GameSceneObjectsReferences.MainCameraController.CameraOffset(null, playerTurn, _rb, _bomber.transform.position.y - 2, 2);
    }

    private IEnumerator PlayAirSirenSound()
    {
        SoundController.MusicSRCVolume(SoundController.MusicVolume.Down);
        SoundController.PlaySound(3, 0, out float clipLength);

        yield return new WaitForSeconds(clipLength);

        SoundController.MusicSRCVolume(SoundController.MusicVolume.Up);
    }
}
