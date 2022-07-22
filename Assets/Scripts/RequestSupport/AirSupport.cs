using UnityEngine;

public class AirSupport : MonoBehaviour, ITurnController
{
    [SerializeField]
    private Bomber _bomber;
    public TurnController TurnController { get; set; }


    private void Awake()
    {        
        TurnController = FindObjectOfType<TurnController>();
    }

    public void Call(out Bomber bomber, Vector3 position, Quaternion rotation, float distanceX)
    {
        _bomber.transform.position = position;
        _bomber.transform.rotation = rotation;
        _bomber.distanceX = distanceX;
        bomber = _bomber;
        TurnController.SetNextTurn(TurnState.Other);
        _bomber.gameObject.SetActive(true);
    }
}
