using UnityEngine;

public class MatchmakeLastStep : MonoBehaviour, IReset
{
    private SubTab _subTab;
    private MatchmakeLastSubTab _lastSubTab;
    private Tab_Matchmake _tabMatchmake;
    private LockScreen _lockScreen;

    private IMatchmakeData[] _iMatchmakeDatas;
    private MatchmakeData _matchmakeData;

    private int _readedFilesCount;


    private void Awake()
    {
        _lastSubTab = Get<MatchmakeLastSubTab>.From(gameObject);
        _subTab = Get<SubTab>.From(gameObject);
        _tabMatchmake = Get<Tab_Matchmake>.From(gameObject);
        _lockScreen = Get<LockScreen>.FromChild(_tabMatchmake.gameObject);
        _iMatchmakeDatas = _tabMatchmake.GetComponentsInChildren<IMatchmakeData>();
    }

    private void OnEnable()
    {
        _lastSubTab._onConfirmReady += SetEverythingUp;
        _subTab._onActivity += GetSubTabActivity;
    }

    private void OnDisable()
    {
        _lastSubTab._onConfirmReady -= SetEverythingUp;
        _subTab._onActivity -= GetSubTabActivity;
    }

    private void SetEverythingUp()
    {
        GetStoredData();
        CreateGame();
    }

    private void GetStoredData()
    {
        _matchmakeData = new MatchmakeData();       

        GlobalFunctions.Loop<IMatchmakeData>.Foreach(_iMatchmakeDatas, iMatchmakeData =>
        {
            iMatchmakeData.StoreData(_matchmakeData);

            _readedFilesCount--;
        });
    }

    private void CreateGame()
    {
        if (_readedFilesCount <= 0)
        {
            _lockScreen.Lock();
            Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, MatchmakeOffline, MatchmakeOnline);
        }
    }

    private void MatchmakeOffline() => MyScene.Manager.LoadScene(MyScene.SceneName.Game);

    private void MatchmakeOnline()
    {
        if (_matchmakeData.IsRoomNameSet)
        {
            MyPhoton.CreateRoom(Photon.Realtime.LobbyType.Default, "Default", _matchmakeData);
        }
    }  

    private void GetSubTabActivity(bool isActive)
    {
        if (!isActive)
        {
            SetDefault();
        }
    }

    public void SetDefault()
    {
        _readedFilesCount = _iMatchmakeDatas.Length;
    } 
}
