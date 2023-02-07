using UnityEngine;

public class Tab_GameStart : MonoBehaviour
{
    [SerializeField] 
    private Tab_GameStartPlayerInfo[] _tabGameStartPlayersInfo;

    [SerializeField] [Space]
    private GameManager _gameManager;


    private void Awake() => InitializePlayesrInfoTab();

    private void OnEnable() => _gameManager.OnGameStarted += OnGameStarted;

    private void OnDisable() => _gameManager.OnGameStarted -= OnGameStarted;

    public void InitializePlayesrInfoTab()
    {
        int _index = 0;

        GlobalFunctions.Loop<Tab_GameStartPlayerInfo>.Foreach(_tabGameStartPlayersInfo, info => 
        {
            if (MyPhotonNetwork.IsOfflineMode)
            {
                if(_index == 0)
                    info.InitializePlayerInfoTab(null, Data.Manager.AvailableTanks[Data.Manager.SelectedTankIndex]._iconTank, "You");
                else
                    info.InitializePlayerInfoTab(null, Data.Manager.AvailableAITanks[Data.Manager.SelectedAITankIndex]._iconTank, "CPU");
            }

            _index++;
        });
    }

    private void OnGameStarted()
    {
        gameObject.SetActive(false);
    }
}
