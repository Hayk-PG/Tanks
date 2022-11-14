using UnityEngine;
using TMPro;
using System;

public class MatchmakeLastSubTab : MonoBehaviour, IReset
{
    [SerializeField] private TMP_Text _text;

    //private Tab_Matchmake _tabMatchmake;
    private SubTab _subTab;
    private IMatchmakeTextResult[] _iMatchmakeTextResult;

    private string _textOnlineResult;
    private string _textOfflineResult;
    private string _textFinalResult;
    private string _textLine;
    private char _newLine = '\n';
    private bool _isActive;
    private float _time;
    private int _charIndex;

    public event Action _onConfirmReady;


    private void Awake()
    {
        //_tabMatchmake = FindObjectOfType<Tab_Matchmake>();
        _subTab = Get<SubTab>.From(gameObject);
        //_iMatchmakeTextResult = _tabMatchmake.GetComponentsInChildren<IMatchmakeTextResult>();  
    }

    private void OnEnable()
    {
        _subTab._onActivity += GetSubTabActivity;
    }

    private void OnDisable()
    {
        _subTab._onActivity -= GetSubTabActivity;
    }

    private void Update()
    {
        ReadTextFinalResult();
    }

    private void ReadTextFinalResult()
    {
        if (_isActive && _charIndex < _textFinalResult.Length)
        {
            _time += 10 * Time.deltaTime;

            bool isNextCharReady = _time >= 0.1f;
            bool isReadyToPrint = _textFinalResult[_charIndex] == _newLine;

            AssembleTextLine(isNextCharReady, isReadyToPrint);
            Finish();
        }
    }

    private void AssembleTextLine(bool isNextCharReady, bool isReadyToPrint)
    {
        if (isNextCharReady)
        {
            _textLine += _textFinalResult[_charIndex];

            PrintAndGoNextLine(isReadyToPrint);
        }
    }

    private void PrintAndGoNextLine(bool isReadyToPrint)
    {
        if (isReadyToPrint)
        {
            _text.text += _textLine;
            _textLine = "";
        }

        _time = 0;
        _charIndex++;
    }

    private void Finish()
    {
        if (_charIndex >= _textFinalResult.Length)
        {
            _onConfirmReady?.Invoke();
        }
    }

    private void GetTextResult()
    {
        foreach (var storedData in _iMatchmakeTextResult)
        {
            _textOnlineResult += storedData.TextResultOnline();
            _textOfflineResult += storedData.TextResultOffline();
        }

        _textFinalResult = MyPhotonNetwork.IsOfflineMode ? _textOfflineResult : _textOnlineResult;
    }

    private void GetSubTabActivity(bool isActive)
    {
        _isActive = isActive;

        Conditions<bool>.Compare(_isActive, GetTextResult, SetDefault);
    }

    public void SetDefault()
    {
        _text.text = "";
        _textLine = "";
        _textOnlineResult = "";
        _textOfflineResult = "";
        _isActive = false;
        _time = 0;
        _charIndex = 0;
    }
}
