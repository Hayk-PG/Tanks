using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TempPoints : MonoBehaviour
{
    private RectTransform _rectTransform;

    private Animator _animator;
    private CanvasGroup _canvasGroup;
    private Text _pointsText;

    private const string _tempPointsAnim = "TempPointsAnim";

    private ScoreController _localPlayerScoreController;
    private HUDScore[] _hudScores;
    private HUDScore _activeHudScore;

    public Action OnTempPointsMotionSoundFX { get; set; }
    public Action<int> OnScoreTextUpdated { get; set; }

    

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _animator = GetComponent<Animator>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _pointsText = GetComponentInChildren<Text>();

        _hudScores = FindObjectsOfType<HUDScore>();
    }

    private void OnDisable()
    {
        if (_localPlayerScoreController != null) _localPlayerScoreController.OnDisplayTempPoints -= OnDisplayTemPoints;
    }

    public void CallPlayerEvents(ScoreController scoreController)
    {
        _localPlayerScoreController = scoreController;

        if(_localPlayerScoreController != null) _localPlayerScoreController.OnDisplayTempPoints += OnDisplayTemPoints;
    }

    private void OnDisplayTemPoints(int score, float waitForSeconds)
    {
        StartCoroutine(Coroutine(score, waitForSeconds));
    }

    private void GetActiveHudScore()
    {
        if (_activeHudScore == null)
        {
            GlobalFunctions.Loop<HUDScore>.Foreach(_hudScores, hudscore =>
            {
                if (hudscore.gameObject.activeInHierarchy)
                    _activeHudScore = hudscore;
            });
        }
    }

    private IEnumerator Coroutine(int score, float waitForSeconds)
    {
        GetActiveHudScore();

        _rectTransform.position = _activeHudScore.TempPointsStartPoint.position;
        string operat = score > 0 ? "+" : "";
        _pointsText.text = operat + score;
        _canvasGroup.alpha = 1;
        _animator.Play(_tempPointsAnim);

        float magnitude = 0;
        bool isReachedToTheDestination = false;

        Vector3 destination;

        yield return new WaitForSeconds(waitForSeconds);

        OnTempPointsMotionSoundFX?.Invoke();

        while (!isReachedToTheDestination)
        {
            destination = _activeHudScore.ScoreTextTransform.position;
            transform.position = Vector2.Lerp(transform.position, destination, 10 * Time.deltaTime);

            magnitude = (destination - transform.position).magnitude;

            if (magnitude <= 100)
            {
                _canvasGroup.alpha -= 100 * Time.deltaTime;

                if (_canvasGroup.alpha <= 0.1f)
                {
                    _canvasGroup.alpha = 0;
                    OnScoreTextUpdated?.Invoke(score);
                    isReachedToTheDestination = true;
                }
            }

            yield return null;
        }
    }
}
