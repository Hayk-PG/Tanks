using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _txt;

    [SerializeField] [Space]
    private Animator _anim;

    private ScoreController _playerScoreController;

    private int _score = 0;



    private void Awake() => UpdateScoreText();

    private void OnEnable() => GameSceneObjectsReferences.GameManager.OnGameStarted += GetPlayerScoreController;

    private void OnDisable() => GameSceneObjectsReferences.GameManager.OnGameStarted -= GetPlayerScoreController;

    private void GetPlayerScoreController()
    {
        _playerScoreController = GlobalFunctions.ObjectsOfType<TankController>.Find(tc => tc.BasePlayer != null)?.GetComponent<ScoreController>();

        _playerScoreController.onDisplayPlayerScore += OnDisplayPlayerScore;
    }

    private void OnDisplayPlayerScore(int score, float waitForSeconds = 0)
    {
        _score += score;

        UpdateScoreText();

        PlayAnimation();
    }

    private void UpdateScoreText()
    {
        if (_txt == null)
            return;

        _txt.text = Converter.DecimalString(_score, 5);
    }

    private void PlayAnimation()
    {
        if (_anim == null)
            return;

        _anim.SetTrigger(Names.Play);
    }
}
