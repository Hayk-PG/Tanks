using UnityEngine;

public class TopBarFromTab_SelectedTanks : MonoBehaviour
{
    private Tab_SelectedTanks _tabSelectedTanks;
    private Tab_StartGame _tabStartGame;
    private Tab_InRoom _tabInRoom;
    [SerializeField] private TopBarResourcesContainer _topBarResourcesContainer;


    private void Awake()
    {
        _tabSelectedTanks = Get<Tab_SelectedTanks>.From(gameObject);
        _tabStartGame = FindObjectOfType<Tab_StartGame>();
        _tabInRoom = FindObjectOfType<Tab_InRoom>();
    }

    private void OnEnable()
    {
        _tabSelectedTanks.OnTabOpened += OnTabSelectedTanksOpen;
    }

    private void OnDisable()
    {
        _tabSelectedTanks.OnTabOpened -= OnTabSelectedTanksOpen;
    }

    private void OnTabSelectedTanksOpen()
    {
        if (MyPhotonNetwork.IsOfflineMode) _topBarResourcesContainer.gameObject.SetActive(false);
        if (!MyPhotonNetwork.IsOfflineMode) _topBarResourcesContainer.gameObject.SetActive(true);
    }

    public void OnTopBarBackButton()
    {
        if (MyPhotonNetwork.IsInRoom)
            _tabInRoom.OpenTab();
        else
            _tabStartGame.OpenTab();
    }
}
