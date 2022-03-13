using System;
using System.Collections;
using UnityEngine;

public class TempPoints : MonoBehaviour
{
    private Animator _animator;
    private CanvasGroup _canvasGroup;
    private GameManager _gameManager;

    private const string _tempPointsAnim = "TempPointsAnim";

    private ScoreController _localPlayerScoreController;
    private HUDScore _hudScore;

    private int _localPlayerScoreTextIndex;

    public event Action<int, int> OnUpdateScore;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _canvasGroup = GetComponent<CanvasGroup>();
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
        if (_localPlayerScoreController != null) _localPlayerScoreController.OnDisplayTemPoints -= OnDisplayTemPoints;
    }

    private void GetPlayerOnGameStart()
    {
        _localPlayerScoreController = GlobalFunctions.ObjectsOfType<PlayerTurn>.Find(p => p.tag == Tags.Player).GetComponent<ScoreController>();
        if (_localPlayerScoreController != null) _localPlayerScoreController.OnDisplayTemPoints += OnDisplayTemPoints;
    }

    private void OnDisplayTemPoints(int score, TurnState localPlayerTurn, Vector3 position)
    {
        if(localPlayerTurn == TurnState.Player1)
        {
            _localPlayerScoreTextIndex = localPlayerTurn == TurnState.Player1 ? 0: 1;

            StartCoroutine(Coroutine(score, position));
        }
    }

    private IEnumerator Coroutine(int score, Vector3 position)
    {
        transform.position = CameraSight.ScreenPoint(position) + new Vector3(0, 100);
        _canvasGroup.alpha = 1;
        _animator.Play(_tempPointsAnim);

        float magnitude = 0;
        bool isReachedToTheDestination = false;

        Vector3 destination;

        yield return new WaitForSeconds(0.5f);

        while (!isReachedToTheDestination)
        {
            destination = _hudScore._scoresTransform[_localPlayerScoreTextIndex]._scorePosition;
            transform.position = Vector2.Lerp(transform.position, destination, 10 * Time.deltaTime);

            magnitude = (destination - transform.position).magnitude;

            if (magnitude <= 100)
            {
                _canvasGroup.alpha -= 100 * Time.deltaTime;

                if(_canvasGroup.alpha <= 0.1f)
                {
                    _canvasGroup.alpha = 0;
                    OnUpdateScore?.Invoke(score, _localPlayerScoreTextIndex);
                    isReachedToTheDestination = true;
                }
            }
          
            yield return null;
        }
    }
}
