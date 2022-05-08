using UnityEngine;

public partial class Tab_EndGame : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private BaseEndGame _baseEndGame;
    private delegate bool Checker(TankController tank);
    private Checker _successed;
    private Checker _defeated;
    

    private void Awake()
    {
        _canvasGroup = Get<CanvasGroup>.From(gameObject);
        _baseEndGame = MyPhotonNetwork.IsOfflineMode ? (BaseEndGame)FindObjectOfType<EndOfflineGame>() : FindObjectOfType<EndOnlineGame>();

        _successed = delegate (TankController successedTank) { return MyPhotonNetwork.IsOfflineMode && successedTank?.BasePlayer != null || !MyPhotonNetwork.IsOfflineMode && successedTank?.BasePlayer != null && successedTank.BasePlayer.photonView.IsMine; };
        _defeated = delegate (TankController defeatedTank) { return MyPhotonNetwork.IsOfflineMode && defeatedTank?.BasePlayer != null || !MyPhotonNetwork.IsOfflineMode && defeatedTank?.BasePlayer != null && defeatedTank.BasePlayer.photonView.IsMine; };
    }

    private void OnEnable()
    {
        _baseEndGame.OnEndGameTab += OnEndGameTab;
    }

    private void OnDisable()
    {
        _baseEndGame.OnEndGameTab -= OnEndGameTab;
    }

    private void OnEndGameTab(string successedPlayerName, string defeatedPlayerName)
    {
        TankController successedTank = GameObject.Find(successedPlayerName)?.GetComponent<TankController>();
        TankController defeatedTank = GameObject.Find(defeatedPlayerName)?.GetComponent<TankController>();
        OnGameEndScreen(successedTank, defeatedTank);
    }

    private void OnGameEndScreen(TankController successedTank, TankController defeatedTank)
    {
        ScoreController successedTanksScore = successedTank?.GetComponent<ScoreController>();
        ScoreController defeatedTanksScore = defeatedTank?.GetComponent<ScoreController>();

        if (_successed(successedTank))
        {
            print("successedTank");
            Display(_ui.colorTitleGlow[0], "Victory", new Values(Data.Manager.Level, 50, 150, 300, (int)(_ui._sliderXP.value), successedTanksScore.Score));          
        }
        else if(_defeated(defeatedTank))
        {
            print("defeatedTank");
            Display(_ui.colorTitleGlow[1], "Defeat", new Values(Data.Manager.Level, 50, 150, 0, (int)(_ui._sliderXP.value), defeatedTanksScore.Score));
        }
    }

    private void Display(Color colorTitleGlow, string textTitle, Values values)
    {
        SetImageTitleGlowColor(colorTitleGlow);
        SetTitleText(textTitle);
        StartCoroutine(DisplayController(_isCoroutineRunning, values));
    }
}
