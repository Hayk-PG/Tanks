using UnityEngine;

public class UpdatePlayerCustomPropertiesOnLocalCallbacks : MonoBehaviour
{
    private MyPhotonCallbacks _myphotonCallbacks;


    private void Awake()
    {
        _myphotonCallbacks = Get<MyPhotonCallbacks>.From(gameObject);
    }

    private void OnEnable()
    {
        _myphotonCallbacks._OnLeftRoom += DeletePlayerReadyKey;
    }

    private void OnDisable()
    {
        _myphotonCallbacks._OnLeftRoom -= DeletePlayerReadyKey;
    }

    private void DeletePlayerReadyKey()
    {
        CustomProperties.Delete(MyPhotonNetwork.LocalPlayer, Keys.IsPlayerReady);
    }
}
