using System;
using UnityEngine;
using UnityEngine.UI;

public class Tab_EndGame : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private BaseEndGame _baseEndGame;

    [Serializable] private struct UI
    {
        [SerializeField] internal Color[] colorTitleGlow;
        [SerializeField] internal Image _imageTitleGlow;
        [SerializeField] internal Text _textTitle;
        [SerializeField] internal Text _textXP;
        [SerializeField] internal Text _textLevel;
        [SerializeField] internal Slider _sliderXP;
    }
    [SerializeField] UI _ui;

    private delegate bool Checker(TankController tank);
    private Checker _successed;
    private Checker _defeated;
    

    private void Awake()
    {
        _canvasGroup = Get<CanvasGroup>.From(gameObject);
        _baseEndGame = FindObjectOfType<BaseEndGame>();

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
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, true);
    }

    private void OnGameEndScreen(TankController successedTank, TankController defeatedTank)
    {
        if(_successed(successedTank))
        {
            //Display
        }
        else if(_defeated(defeatedTank))
        {
            //Display
        }
    }

    private void Display(Color colorTitleGlow, string textTitle, int level)
    {
        _ui._imageTitleGlow.color = colorTitleGlow;
        _ui._textTitle.text = textTitle;
        _ui._textLevel.text = level.ToString();
    }
}
