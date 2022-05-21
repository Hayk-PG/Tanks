using UnityEngine;
using UnityEngine.UI;

public class Button_MinimizedCurrentRoom : BaseButtonWithUnityEvent
{
    private MyPhotonCallbacks _myPhotonCallbacks;

    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Text _textCurrentRoomName;
    [SerializeField] private Text _textCurrentRoomPlayersCount;


    protected override void Awake()
    {
        base.Awake();
        _myPhotonCallbacks = FindObjectOfType<MyPhotonCallbacks>();
    }

    private void OnEnable()
    {
        _myPhotonCallbacks._OnLeftRoom += () => GlobalFunctions.CanvasGroupActivity(_canvasGroup, false);
    }

    private void OnDisable()
    {
        _myPhotonCallbacks._OnLeftRoom -= ()=> GlobalFunctions.CanvasGroupActivity(_canvasGroup, false);
    }

    private void Update()
    {
        if (MyPhotonNetwork.IsInRoom)
        {
            if (MyPhotonNetwork.CurrentRoom.Name.Length > 7)
                _textCurrentRoomName.text = MyPhotonNetwork.CurrentRoom.Name.Substring(0, 7) + "...";
            else
                _textCurrentRoomName.text = MyPhotonNetwork.CurrentRoom.Name;

            _textCurrentRoomPlayersCount.text = MyPhotonNetwork.CurrentRoom.PlayerCount + "/" + MyPhotonNetwork.CurrentRoom.MaxPlayers;
        }
    }

    public void OpenTab_MinimizedCurrentRoom()
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, true);
    }

    public override void OnClick()
    {
        base.OnClick();
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, false);
    }
}
