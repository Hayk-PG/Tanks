using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour
{
    private Animator _anim;
    private TMP_Text _txt;

    private int Score
    {
        get => int.Parse(_txt.text);
        set => _txt.text = value.ToString();
    }


    private void Awake()
    {
        _anim = Get<Animator>.From(gameObject);
        _txt = Get<TMP_Text>.From(gameObject);
    }

    public void UpdateScoreText(int score)
    {
        Score += score;

        if (_anim != null)
            _anim.SetTrigger(Names.Play);
    }
}
