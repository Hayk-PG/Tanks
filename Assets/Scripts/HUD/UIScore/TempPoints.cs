using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TempPoints : MonoBehaviour
{
    private Animator _animator;
    private CanvasGroup _canvasGroup;
    private Text _pointsText;
    private GameManager _gameManager;
    private HUDScore _hudScore;
    private ScoreController[] _playersScoreControllers;

    private const string _tempPointsAnim = "TempPointsAnim";

    private int _playerScoreTextIndex;

    public event Action OnPlayerTempPoints;
    public event Action<int, int> OnUpdateScore;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _pointsText = GetComponentInChildren<Text>();
        _gameManager = FindObjectOfType<GameManager>();
        _hudScore = FindObjectOfType<HUDScore>();
    }

    private void OnEnable()
    {
        _gameManager.OnGameStarted += GetPlayerOnGameStart;
    }

    private void OnDisable()
    {
        _gameManager.OnGameStarted -= GetPlayerOnGameStart;

        if (_playersScoreControllers != null)
        {
            foreach (var scoreController in _playersScoreControllers)
            {
                scoreController.OnDisplayTemPoints -= OnDisplayTemPoints;
            }
        }
    }

    private void GetPlayerOnGameStart()
    {
        _playersScoreControllers = FindObjectsOfType<ScoreController>();

        if (_playersScoreControllers != null)
        {
            foreach (var scoreController in _playersScoreControllers)
            {
                scoreController.OnDisplayTemPoints += OnDisplayTemPoints;
            }
        }
    }

    private void OnDisplayTemPoints(int score, TurnState localPlayerTurn, Vector3 position)
    {
        _playerScoreTextIndex = localPlayerTurn == TurnState.Player1 ? 0 : 1;

        if (localPlayerTurn == TurnState.Player1)
            StartCoroutine(Coroutine(score, position));
        else
            OnUpdateScore?.Invoke(score, _playerScoreTextIndex);

    }

    private IEnumerator Coroutine(int score, Vector3 position)
    {
        transform.position = CameraSight.ScreenPoint(position) + new Vector3(0, 100);
        _pointsText.text = "+" + score;
        _canvasGroup.alpha = 1;
        _animator.Play(_tempPointsAnim);

        float magnitude = 0;
        bool isReachedToTheDestination = false;

        Vector3 destination;

        yield return new WaitForSeconds(0.5f);

        OnPlayerTempPoints?.Invoke();

        while (!isReachedToTheDestination)
        {
            destination = _hudScore._scoresTransform[_playerScoreTextIndex]._scorePosition;
            transform.position = Vector2.Lerp(transform.position, destination, 10 * Time.deltaTime);

            magnitude = (destination - transform.position).magnitude;

            if (magnitude <= 100)
            {
                _canvasGroup.alpha -= 100 * Time.deltaTime;

                if(_canvasGroup.alpha <= 0.1f)
                {
                    _canvasGroup.alpha = 0;
                    OnUpdateScore?.Invoke(score, _playerScoreTextIndex);
                    isReachedToTheDestination = true;
                }
            }
          
            yield return null;
        }
    }
}
