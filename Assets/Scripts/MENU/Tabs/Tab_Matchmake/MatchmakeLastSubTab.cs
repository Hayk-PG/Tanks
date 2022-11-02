using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class MatchmakeLastSubTab : MonoBehaviour, IReset
{
    [SerializeField] private TMP_Text _text;

    private Tab_Matchmake _tabMatchmake;
    private SubTab _subTab;
    private IMatchmakeTextResult[] _iMatchmakeTextResult;

    private string _dataInOnlineMode;
    private string _dataInOfflineMode;
    private string _combinedText;
    private string textPiece;
    private char _newLine = '\n';
    private bool _isActive;
    private float _time;
    private int _charIndex;

    public event Action _onConfirmReady;


    private void Awake()
    {
        _tabMatchmake = FindObjectOfType<Tab_Matchmake>();
        _subTab = Get<SubTab>.From(gameObject);
        _iMatchmakeTextResult = _tabMatchmake.GetComponentsInChildren<IMatchmakeTextResult>();  
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
        if (_isActive && _charIndex < _combinedText.Length)
        {
            _time += 2 * Time.deltaTime;

            if (_time >= 0.1f)
            {
                textPiece += _combinedText[_charIndex];

                if(_combinedText[_charIndex] == _newLine)
                {
                    _text.text += textPiece;
                    textPiece = "";
                }
            
                _time = 0;
                _charIndex++;
            }

            if(_charIndex >= _combinedText.Length)
            {
                _onConfirmReady?.Invoke();
            }
        }
    }

    private void GetMessage()
    {
        foreach (var storedData in _iMatchmakeTextResult)
        {
            _dataInOnlineMode += storedData.TextResultOnline();
            _dataInOfflineMode += storedData.TextResultOffline();
        }

        _combinedText = MyPhotonNetwork.IsOfflineMode ? _dataInOfflineMode : _dataInOnlineMode;
    }

    private void GetSubTabActivity(bool isActive)
    {
        _isActive = isActive;

        Conditions<bool>.Compare(_isActive, GetMessage, SetDefault);
    }

    public void SetDefault()
    {
        _text.text = "";
        textPiece = "";
        _dataInOnlineMode = "";
        _dataInOfflineMode = "";
        _isActive = false;
        _time = 0;
        _charIndex = 0;
    }
}
