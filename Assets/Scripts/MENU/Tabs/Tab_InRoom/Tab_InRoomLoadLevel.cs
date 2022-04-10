using UnityEngine;
using UnityEngine.UI;

public class Tab_InRoomLoadLevel : MonoBehaviour
{
    private MyPlugins _myPlugins;

    [SerializeField] private PlayerInRoom[] _playersInRoom;

    [SerializeField] private Text _textTest;
    private int _s;

    private bool _isLoaded;

    private void Awake()
    {
        _myPlugins = FindObjectOfType<MyPlugins>();
    }

    private void OnEnable()
    {
        _myPlugins.OnPluginService += OnPluginService;
    }

    private void OnDisable()
    {
        _myPlugins.OnPluginService -= OnPluginService;
    }

    private void OnPluginService()
    {
        _s++;
        _textTest.text += _s.ToString() + "/ " + Photon.Pun.PhotonNetwork.Time + "\n";

        //if (!_isLoaded)
        //{
        //    MyPhotonNetwork.LoadLevel();
            
        //    _isLoaded = true;
        //}

        

        //int readyPlayersCount = 0;

        //for (int i = 0; i < _playersInRoom.Length; i++)
        //{
        //    if (_playersInRoom[i].IsPlayerReady) readyPlayersCount++;
        //}

        //if (readyPlayersCount == MyPhotonNetwork.PlayersList.Length)
        //{
        //    MyPhotonNetwork.LoadLevel();
        //    _myPlugins.OnPluginService -= OnPluginService;
        //}
    }
}
