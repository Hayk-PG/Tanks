using UnityEngine;
using UnityEngine.UI;

public class MatchmakeLastStep : MonoBehaviour, IReset
{
    [SerializeField] private Button _confirmButton;
    private SubTab _subTab;
    private MatchmakeLastSubTab _lastSubTab;
    private Tab_Matchmake _tabMatchmake;

    private IMatchmakeData[] _iMatchmakeDatas;
    private MatchmakeData _matchmakeData;


    private void Awake()
    {
        _lastSubTab = Get<MatchmakeLastSubTab>.From(gameObject);
        _subTab = Get<SubTab>.From(gameObject);
        _tabMatchmake = Get<Tab_Matchmake>.From(gameObject);
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

    private void Update()
    {
        _confirmButton.onClick.RemoveAllListeners();
        _confirmButton.onClick.AddListener(Click);
    }

    private void SetEverythingUp()
    {
        GetStoredData();
        _confirmButton.interactable = true;
    }

    private void GetStoredData()
    {
        _matchmakeData = new MatchmakeData();
        GlobalFunctions.Loop<IMatchmakeData>.Foreach(_iMatchmakeDatas, iMatchmakeData => { iMatchmakeData.StoreData(_matchmakeData); });
    }

    private void GetSubTabActivity(bool isActive)
    {
        if (!isActive)
        {
            SetDefault();
        }
    }

    public void SetDefault() => _confirmButton.interactable = false;

    public void Click() => Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, MatchmakeOffline, MatchmakeOnline);

    private void MatchmakeOffline() => MyScene.Manager.LoadScene(MyScene.SceneName.Game);

    private void MatchmakeOnline()
    {
        if (_matchmakeData.IsRoomNameSet)
        {
            MyPhoton.CreateRoom(Photon.Realtime.LobbyType.Default, "Default", _matchmakeData);
        }
    }
}
