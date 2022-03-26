using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempPoints : MonoBehaviour
{
    private Animator _animator;
    private CanvasGroup _canvasGroup;
    private Text _pointsText;
    private HUDScore _hudScore;
    private TurnController _turnController;

    private const string _tempPointsAnim = "TempPointsAnim";

    public event Action OnPlayerTempPoints;
    public event Action<int> OnUpdateScore;

    private ScoreController _localPlayerScoreController;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _pointsText = GetComponentInChildren<Text>();
        _turnController = FindObjectOfType<TurnController>();
        _hudScore = FindObjectOfType<HUDScore>();
    }

    private void OnEnable()
    {
        _turnController.OnPlayers += SubscribeToScoreController;
    }

    private void OnDisable()
    {
        _turnController.OnPlayers -= SubscribeToScoreController;

        if (_localPlayerScoreController != null)
            _localPlayerScoreController.OnDisplayTempPoints -= OnDisplayTemPoints;
    }

    private void SubscribeToScoreController(List<PlayerTurn> localPlayer)
    {
        //PHOTON

        if(localPlayer.Find(localplayer => localplayer.tag == Tags.Player) != null)
        {
            _localPlayerScoreController = localPlayer.Find(localplayer => localplayer.tag == Tags.Player).GetComponent<ScoreController>();
            _localPlayerScoreController.OnDisplayTempPoints += OnDisplayTemPoints;
        }
    }

    private void OnDisplayTemPoints(int score, Vector3 position)
    {
        StartCoroutine(Coroutine(score, position));
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
            destination = _hudScore.ScoreTextTransform.position;
            transform.position = Vector2.Lerp(transform.position, destination, 10 * Time.deltaTime);

            magnitude = (destination - transform.position).magnitude;

            if (magnitude <= 100)
            {
                _canvasGroup.alpha -= 100 * Time.deltaTime;

                if (_canvasGroup.alpha <= 0.1f)
                {
                    _canvasGroup.alpha = 0;
                    OnUpdateScore?.Invoke(score);
                    isReachedToTheDestination = true;
                }
            }

            yield return null;
        }
    }
}
