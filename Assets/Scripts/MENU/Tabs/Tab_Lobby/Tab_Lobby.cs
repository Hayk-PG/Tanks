using Photon.Realtime;
using UnityEngine;

public class Tab_Lobby : Tab_Base<MyPhotonCallbacks>
{
    private Tab_SelectedTanks _tab_selectedTanks;

    [SerializeField] private Transform _transform_Content;
    [SerializeField] private RoomButton _prefab_roomButton;

    private RoomButton _copy_roomButton;


    protected override void Awake()
    {
        base.Awake();
        _tab_selectedTanks = FindObjectOfType<Tab_SelectedTanks>();
    }

    private void OnEnable()
    {
        _tab_selectedTanks.OnPhotonOnlineTankSelected += base.OpenTab;
        _object._OnRoomListUpdate += OnUpdateRoomList;
    }

    private void OnDisable()
    {
        _tab_selectedTanks.OnPhotonOnlineTankSelected -= base.OpenTab;
        _object._OnRoomListUpdate -= OnUpdateRoomList;
    }

    private void OnUpdateRoomList(RoomInfo roomInfo)
    {
        if(_transform_Content.Find(roomInfo.Name) == null)
        {
            _copy_roomButton = Instantiate(_prefab_roomButton, _transform_Content);
            _copy_roomButton.AssignRoomButton(new RoomButton.UI(roomInfo.Name, "---", roomInfo.PlayerCount + "/" + roomInfo.MaxPlayers, null));
        }
        else
        {
            if (roomInfo.RemovedFromList)
            {
                Destroy(_transform_Content.Find(roomInfo.Name).gameObject);
            }
        }        
    }
}
